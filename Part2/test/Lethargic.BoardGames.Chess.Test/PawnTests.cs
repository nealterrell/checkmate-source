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
		public void NoBackwardsCapture() {
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
		/// Pawn capture.
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
			b.CurrentAdvantage.Should().Be(Advantage(2, 1), "Black lost a single pawn of 1 value");

			b.UndoLastMove();
			b.CurrentAdvantage.Should().Be(Advantage(0, 0), "after undoing the pawn capture, advantage is neutral");
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
				"b5, a6" // capture rook with pawn
			);
			b.CurrentAdvantage.Should().Be(Advantage(1, 5), "a Black rook was captured");

			Apply(b,
				"b7, b6",
				"a6, a7"
			);
			b.CurrentAdvantage.Should().Be(Advantage(1, 5), "no other pieces captured");

			Apply(b, "b6", "b5");
			b.CurrentAdvantage.Should().Be(Advantage(1, 5), "no other pieces captured");

			var possMoves = b.GetPossibleMoves();
			possMoves.Should().HaveCount(4, "there are four possible promotion moves")
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

			b.UndoLastMove(); // this undoes the pawn's movement to the final rank
			b.CurrentPlayer.Should().Be(1, "undoing the pawn's final movement should NOT change current player");
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
				.And.Contain(Move("a5, b6"));

			// Apply the en passant
			Apply(b, Move("a5, b6"));
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
