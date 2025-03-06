using Parser.Map;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V3.Grid;
using System;

namespace Parser.Utils
{
    public static class MapExtensions
    {
		public const int NumberOfLines = 4;

		public static Note.Direction horizontal_cut_transform(Note.Direction direction) {
			switch (direction) {
				case Note.Direction.UpLeft:
					return Note.Direction.UpRight;
				case Note.Direction.DownLeft:
					return Note.Direction.DownRight;
				case Note.Direction.UpRight:
					return Note.Direction.UpLeft;
				case Note.Direction.DownRight:
					return Note.Direction.DownLeft;
				case Note.Direction.Left:
					return Note.Direction.Right;
				case Note.Direction.Right:
					return Note.Direction.Left;
				default:
					break;
			}

			return direction;
		}

		public static void Mirror(this BeatmapColorGridObject self) {
			var color = self.Color;
			if (self.Color == 0) {
				color = 1;
			} else if (self.Color == 1) {
				color = 0;
			}

			int flippedX;
			if (self.x <= -1000 || self.x >= 1000) {
				flippedX = (NumberOfLines - 1) * 1000 - self.x;
				if (flippedX >= 0 && flippedX < NumberOfLines * 1000) {
					flippedX += 2000;
				}
			} else {
				flippedX = NumberOfLines - 1 - self.x;
			}

			self.Color = color;
			self.x = flippedX;

			// Handle custom data position
			//if (self.CustomData?.Position != null) {
			//	self.CustomData.Position[0] = NumberOfLines - self.CustomData.Position[0] - 5;
			//}
		}

		public static void Mirror(this Note self) {
			Mirror((BeatmapColorGridObject)self);

			self.CutDirection = (int)horizontal_cut_transform((Note.Direction)self.CutDirection);
			self.AngleOffset = -self.AngleOffset;

			// Handle custom cut direction
			//if (self.CustomData?.CutDirection != null) {
			//	self.CustomData.CutDirection = -self.CustomData.CutDirection;
			//}
		}

		public static void Mirror(this BeatmapColorGridObjectWithTail self) {
			Mirror((BeatmapColorGridObject)self);

            self.CutDirection = (int)horizontal_cut_transform((Note.Direction)self.CutDirection);
            self.TailCutDirection = (int)horizontal_cut_transform((Note.Direction)self.TailCutDirection);

			int flippedTX;
			if (self.tx <= -1000 || self.tx >= 1000) {
				flippedTX = (NumberOfLines - 1) * 1000 - self.tx;
				if (flippedTX >= 0 && flippedTX < NumberOfLines * 1000) {
					flippedTX += 2000;
				}
			} else {
				flippedTX = NumberOfLines - 1 - self.tx;
			}

			self.tx = flippedTX;

			// Handle custom data tail position
			//if (self.CustomData?.TailPosition != null) {
			//	self.CustomData.TailPosition[0] = NumberOfLines - self.CustomData.TailPosition[0] - 5;
			//}
		}

		public static void Mirror(this Wall self) {
			if (self.x <= -1000 || self.x >= 1000) {
				var width = self.Width;
				if (width < 1000) {
					width *= 1000;
				}
				var lineIndex = (NumberOfLines - 1) * 1000 - width - self.x;
				self.x = lineIndex + 2000;
			} else {
				self.x = NumberOfLines - self.Width - self.x;
			}

			// Handle custom data position and width
			//if (self.CustomData?.Position != null) {
			//	self.CustomData.Position[0] = NumberOfLines - self.CustomData.Position[0] - 5;

			//	if (self.Width > 1) {
			//		self.Width *= -1;
			//		self.CustomData.Position[0] += 1;
			//	}
			//}
		}

		public static void Mirror(this DifficultySet self) {

			foreach (var note in self.Data.Notes)
			{
				if (note.Color != 0 && note.Color != 1) {
					note.x = NumberOfLines - 1 - note.x;
				} else {
					note.Mirror();
				}
			}

            foreach (var note in self.Data.Bombs)
			{
				note.x = NumberOfLines - 1 - note.x;
			}

			foreach (var chain in self.Data.Chains)
			{
				chain.Mirror();
			}

			foreach (var arc in self.Data.Arcs)
			{
				arc.Mirror();
			}

			foreach (var wall in self.Data.Walls)
			{
				wall.Mirror();
			}
		}

        public static void Mirror(this BeatmapV3 self) {
			foreach (var diff in self.Difficulties)
			{
				diff.Mirror();
			}
        }
    }
}
