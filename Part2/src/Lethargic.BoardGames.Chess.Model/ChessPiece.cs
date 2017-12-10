﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lethargic.BoardGames.Chess.Model {
	/// <summary>
	/// Represents a chess piece owned by a particular player.
	/// </summary>
	public struct ChessPiece {
		public ChessPieceType PieceType { get; }
		public sbyte Player { get; }

		public ChessPiece(ChessPieceType pieceType, int player) {
			PieceType = pieceType;
			Player = (sbyte)player;
		}
	}
}
