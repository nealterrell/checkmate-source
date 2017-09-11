using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Lethargic.BoardGames.Chess.Model;
using Xunit;

namespace Lethargic.BoardGames.Chess.Test {
	public class PawnTests : ChessTest {
		/// <summary>
		/// Moving pawns one or two spaces.
		/// </summary>
		[Fact]
		public void PawnTwoSpaceMove() {
			ChessBoard b = new ChessBoard();

			var possMoves = b.GetPossibleMoves();
			// Each of the pawns in rank 2 should have two move options.
			foreach (var pos in GetPositionsInRank(2)) {
				var movesAtPos = GetMovesAtPosition(possMoves, pos);
				movesAtPos.Should().HaveCount(2)
					.And.BeEquivalentTo(
						Move(pos, pos.Translate(-1, 0)),
						Move(pos, pos.Translate(-2, 0))
				);
			}
			Apply(b, "a2, a3"); // one space move

			// Same, but for pawns in rank 7
			possMoves = b.GetPossibleMoves();
			foreach (var pos in GetPositionsInRank(7)) {
				var movesAtPos = GetMovesAtPosition(possMoves, pos);
				movesAtPos.Should().HaveCount(2)
					.And.BeEquivalentTo(
						Move(pos, pos.Translate(1, 0)),
						Move(pos, pos.Translate(2, 0))
					);
			}
			Apply(b, "a7, a5"); // player 2 response

			possMoves = b.GetPossibleMoves();
			var oneMoveExpected = GetMovesAtPosition(possMoves, Pos("a3"));
			oneMoveExpected.Should().Contain(Move("a3, a4"))
				.And.HaveCount(1, "a pawn not in its original rank can only move one space forward");

			var twoMovesExpected = GetMovesAtPosition(possMoves, Pos("b2"));
			twoMovesExpected.Should().Contain(Move("b2, b3"))
				.And.Contain(Move("b2, b4"))
				.And.HaveCount(2, "a pawn in its original rank can move up to two spaces forward");
		}

		[Fact]
		public void PawnTwoSpaceMove_IfUnblocked() {
			// Move a pawn from each side to in front of the other's starting pawns.
			ChessBoard b = CreateBoardFromMoves(
				"a2, a4",
				"b7, b5",
				"a4, a5",
				"b5, b4",
				"a5, a6",
				"b4, b3"
			);

			var possMoves = b.GetPossibleMoves();
			var blockedPawn = GetMovesAtPosition(possMoves, Pos("b2"));
			blockedPawn.Should().BeEmpty("The pawn at b2 is blocked by the enemy at b3");
			Apply(b, Move("c2, c4"));

			possMoves = b.GetPossibleMoves();
			blockedPawn = GetMovesAtPosition(possMoves, Pos("a7"));
			blockedPawn.Should().BeEmpty("The pawn at a7 is blocked by the enemy at a6");
		}

		/// <summary>
		/// A pawn cannot make a two space movement if it is on the enemy's starting rank.
		/// </summary>
		[Fact]
		public void PawnTwoSpaceMove_DirectionMatters() {
			ChessBoard b = CreateBoardFromMoves(
				"a2, a4",
				"b7, b5",
				"a4, b5",
				"b8, a6",
				"b5, b6",
				"h7, h5",
				"b6, b7",
				"h5, h4"
			);
			var possMoves = b.GetPossibleMoves();
			var oneMove = GetMovesAtPosition(possMoves, Pos("b7"));
			// The pawn at b7 should have 12 possible moves: 4 promotion moves each, for a move forward,
			// and two capturing moves diagonally.
			oneMove.Should().HaveCount(12)
				.And.NotContain(Move("b7, b9"))
				.And.OnlyContain(m => m.MoveType == ChessMoveType.PawnPromote);
		}

		/// <summary>
		/// Pawn diagonal capture.
		/// </summary>
		[Fact]
		public void PawnCapture() {
			ChessBoard b = CreateBoardFromMoves(
				Move("a2, a4"),
				Move("b7, b5")
			);

			var poss = b.GetPossibleMoves();
			var expected = GetMovesAtPosition(poss, Pos("a4"));
			expected.Should().Contain(Move("a4, b5"))
				.And.Contain(Move("a4, a5"))
				.And.HaveCount(2, "a pawn can capture diagonally ahead or move forward");

			b.CurrentAdvantage.Should().Be(Advantage(0, 0), "no operations have changed the advantage");

			Apply(b, Move("a4, b5"));
			b.GetPieceAtPosition(Pos("b5")).Player.Should().Be(1, "Player 1 captured Player 2's pawn diagonally");
			b.CurrentAdvantage.Should().Be(Advantage(1, 1), "Black lost a single pawn of 1 value");

			b.UndoLastMove();
			b.CurrentAdvantage.Should().Be(Advantage(0, 0), "after undoing the pawn capture, advantage is neutral");
		}

		[Fact]
		public void PawnCapture_EvenIfBlocked() {
			ChessBoard b = CreateBoardFromMoves(
				Move("a2, a4"),
				Move("h7, h5"),
				Move("a4, a5"),
				Move("h5, h4"),
				Move("a5, a6")
			);

			var possMoves = b.GetPossibleMoves();
			var threeMoves = GetMovesAtPosition(possMoves, Pos("b7"));
			threeMoves.Should().HaveCount(3)
				.And.BeEquivalentTo(Move("b7, a6"), Move("b7, b6"), Move("b7, b5"));
		}

		[Fact]
		public void PawnCapture_NoBackwardsCapture() {
			ChessBoard b = CreateBoardFromMoves(
				"a2, a4",
				"b7, b5",
				"a4, a5",
				"c7, c6",
				"a5, a6"
			);

			var possMoves = b.GetPossibleMoves();
			GetMovesAtPosition(possMoves, Pos("b5")).Should().HaveCount(1)
				.And.Contain(Move("b5, b4"));
		}

		/// <summary>
		/// Pawns cannot capture diagonally off the board, wrapping to another row.
		/// </summary>
		[Fact]
		public void PawnBorderCapture() {
			ChessBoard b = CreateBoardFromMoves(
				Move("a2, a4"),
				Move("h7, h5")
			);

			var possMoves = b.GetPossibleMoves();
			var forwardOnly = GetMovesAtPosition(possMoves, Pos("a4"));
			forwardOnly.Should().HaveCount(1)
				.And.Contain(Move("a4, a5"));

			Apply(b, Move("b2, b4"));
			possMoves = b.GetPossibleMoves();
			forwardOnly = GetMovesAtPosition(possMoves, Pos("h5"));
			forwardOnly.Should().HaveCount(1)
				.And.Contain(Move("h5, h4"));
		}

		/// <summary>
		/// Promote a pawn after reaching the final rank.
		/// </summary>
		[Fact]
		public void PawnPromoteTest() {
			ChessBoard b = CreateBoardFromMoves(
				"b2, b4",
				"a7, a5",
				"b4, b5",
				"a8, a6",
				"b5, a6", // capture rook with pawn
				"b8, c6",
				"a6, a7",
				"c6, d4"
			);
			b.CurrentAdvantage.Should().Be(Advantage(1, 5), "a Black rook was captured");

			// Make sure all possible moves are marked PawnPromote.
			var possMoves = b.GetPossibleMoves();
			var pawnMoves = GetMovesAtPosition(possMoves, Pos("a7"));
			pawnMoves.Should().HaveCount(4, "there are four possible promotion moves")
				.And.OnlyContain(m => m.MoveType == ChessMoveType.PawnPromote);

			// Apply the promotion move
			Apply(b, Move("(a7, a8, Queen)"));
			b.GetPieceAtPosition(Pos("a8")).PieceType.Should().Be(ChessPieceType.Queen, "the pawn was replaced by a queen");
			b.GetPieceAtPosition(Pos("a8")).Player.Should().Be(1, "the queen is controlled by player 1");
			b.CurrentPlayer.Should().Be(2, "choosing the pawn promotion should change the current player");
			b.CurrentAdvantage.Should().Be(Advantage(1, 13), "gained 9 points, lost 1 point from queen promotion");

			b.UndoLastMove();
			b.CurrentPlayer.Should().Be(1, "undoing a pawn promotion should change the current player");
			b.CurrentAdvantage.Should().Be(Advantage(1, 5), "lose value of queen when undoing promotion");
		}

		[Fact]
		public void EnPassantTest() {
			ChessBoard b = CreateBoardFromMoves(
				"a2, a4",
				"h7, h5",
				"a4, a5"
			);

			// Move pawn forward twice, enabling en passant from a5
			Apply(b, "b7, b5");
			
			var possMoves = b.GetPossibleMoves();
			var enPassantExpected = GetMovesAtPosition(possMoves, Pos("a5"));
			enPassantExpected.Should().HaveCount(2, "pawn can move forward one or en passant")
				.And.Contain(Move("a5, a6"))
				.And.Contain(Move(Pos("a5"), Pos("b6"), ChessMoveType.EnPassant));

			// Apply the en passant
			Apply(b, Move(Pos("a5"), Pos("b6"), ChessMoveType.EnPassant));
			var pawn = b.GetPieceAtPosition(Pos("b6"));
			pawn.Player.Should().Be(1, "pawn performed en passant move");
			pawn.PieceType.Should().Be(ChessPieceType.Pawn);
			var captured = b.GetPieceAtPosition(Pos("b5"));
			captured.Player.Should().Be(0, "the pawn that moved to b5 was captured by en passant");
			b.CurrentAdvantage.Should().Be(Advantage(1, 1));

			// Undo the move and check the board state
			b.UndoLastMove();
			b.CurrentAdvantage.Should().Be(Advantage(0, 0));
			pawn = b.GetPieceAtPosition(Pos("a5"));
			pawn.Player.Should().Be(1);
			captured = b.GetPieceAtPosition(Pos("b5"));
			captured.Player.Should().Be(2);
			var empty = b.GetPieceAtPosition(Pos("b6"));
			empty.Player.Should().Be(0);
		}
	}
}
