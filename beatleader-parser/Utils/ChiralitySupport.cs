using Parser.Map;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V3.Grid;
using System.Collections.Generic;

namespace Parser.Utils
{
    public class ChiralitySupport
    {
        public static System.Random rand;
        public static List<Note.Direction> directions = new List<Note.Direction> {
            Note.Direction.Up, 
            Note.Direction.Down, 
            Note.Direction.Left, 
            Note.Direction.Right,
            Note.Direction.UpLeft, 
            Note.Direction.UpRight, 
            Note.Direction.DownLeft, 
            Note.Direction.DownRight,
            Note.Direction.Any
        };
        public static Dictionary<Note.Direction, Note.Direction> horizontal_cut_transform = new Dictionary<Note.Direction, Note.Direction>
        {
            { Note.Direction.Up, Note.Direction.Up },
            { Note.Direction.Down, Note.Direction.Down },
            { Note.Direction.UpLeft, Note.Direction.UpRight },
            { Note.Direction.DownLeft, Note.Direction.DownRight },
            { Note.Direction.UpRight, Note.Direction.UpLeft },
            { Note.Direction.DownRight, Note.Direction.DownLeft },
            { Note.Direction.Left, Note.Direction.Right },
            { Note.Direction.Right, Note.Direction.Left },
            { Note.Direction.Any, Note.Direction.Any }
        };
        public static Dictionary<Note.Direction, Note.Direction> vertical_cut_transform = new Dictionary<Note.Direction, Note.Direction>
        {
            { Note.Direction.Up, Note.Direction.Down },
            { Note.Direction.Down, Note.Direction.Up },
            { Note.Direction.UpLeft, Note.Direction.DownLeft },
            { Note.Direction.DownLeft, Note.Direction.UpLeft },
            { Note.Direction.UpRight, Note.Direction.DownRight },
            { Note.Direction.DownRight, Note.Direction.UpRight },
            { Note.Direction.Left, Note.Direction.Left },
            { Note.Direction.Right, Note.Direction.Right },
            { Note.Direction.Any, Note.Direction.Any },
        };

        #region "Main Transform Functions"
        public static DifficultyV3 Mirror_Horizontal(DifficultyV3 beatmapSaveData, int numberOfLines, bool flip_lines, bool remove_walls)
        {
            // Bombs:
            List<Bomb> h_bombNotes = new List<Bomb>();
            foreach (Bomb bomb in beatmapSaveData.Bombs)
            {
                h_bombNotes.Add(Mirror_Horizontal_Bomb(bomb, numberOfLines, flip_lines));
            }

            // ColorNotes:
            List<Note> h_colorNotes = new List<Note>();
            foreach (Note colorNote in beatmapSaveData.Notes)
            {
                h_colorNotes.Add(Mirror_Horizontal_Note(colorNote, numberOfLines, flip_lines));
            }

            // Obstacles:
            List<Wall> h_obstacleDatas = new List<Wall>();
            if (remove_walls == false)
            {
                foreach (Wall obstacleData in beatmapSaveData.Walls)
                {
                    h_obstacleDatas.Add(Mirror_Horizontal_Obstacle(obstacleData, numberOfLines, flip_lines));
                }
            }

            // Sliders:
            List<Arc> h_sliderDatas = new List<Arc>();
            foreach (Arc sliderData in beatmapSaveData.Arcs)
            {
                h_sliderDatas.Add(Mirror_Horizontal_Slider(sliderData, numberOfLines, flip_lines));
            }

            // BurstSliders:
            List<Chain> h_burstSliderDatas = new List<Chain>();
            foreach (Chain burstSliderData in beatmapSaveData.Chains)
            {
                int headcutDirection = burstSliderData.CutDirection;
                var mirroredNote = Mirror_Horizontal_BurstSlider(burstSliderData, numberOfLines, flip_lines);
                if (mirroredNote.CutDirection != headcutDirection) {
                    foreach (var note in h_colorNotes)
                    {
                        if (mirroredNote.x == note.x && mirroredNote.y == note.y && mirroredNote.Beats == note.Beats) {
                            note.x = mirroredNote.tx;
                        }
                    }

                    var tailLineIdex = mirroredNote.tx;
			        mirroredNote.tx = mirroredNote.x;
			        mirroredNote.x = tailLineIdex;
                }

                h_burstSliderDatas.Add(mirroredNote);
            }

            return new DifficultyV3 {
                bpmEvents = beatmapSaveData.bpmEvents,
                Rotations = beatmapSaveData.Rotations,
                Notes = h_colorNotes,
                Bombs = h_bombNotes,
                Walls = h_obstacleDatas,
                Arcs = h_sliderDatas,
                Chains = h_burstSliderDatas,
                Waypoints = beatmapSaveData.Waypoints,
                Lights = beatmapSaveData.Lights,
                colorBoostBeatmapEvents = beatmapSaveData.colorBoostBeatmapEvents,
                lightColorEventBoxGroups = beatmapSaveData.lightColorEventBoxGroups,
                lightRotationEventBoxGroups = beatmapSaveData.lightRotationEventBoxGroups,
                lightTranslationEventBoxGroups = beatmapSaveData.lightTranslationEventBoxGroups,
                vfxEventBoxGroups = beatmapSaveData.vfxEventBoxGroups,
                _fxEventsCollection = beatmapSaveData._fxEventsCollection,
                basicEventTypesWithKeywords = beatmapSaveData.basicEventTypesWithKeywords,
                useNormalEventsAsCompatibleEvents = beatmapSaveData.useNormalEventsAsCompatibleEvents
            };
        }


        public static DifficultyV3 Mirror_Vertical(DifficultyV3 beatmapSaveData, bool flip_rows, bool remove_walls)
        {
            // Bombs:
            List<Bomb> v_bombNotes = new List<Bomb>();
            foreach (Bomb bomb in beatmapSaveData.Bombs)
            {
                v_bombNotes.Add(Mirror_Vertical_Bomb(bomb, flip_rows));
            }

            // ColorNotes:
            List<Note> v_colorNotes = new List<Note>();
            foreach (Note colorNote in beatmapSaveData.Notes)
            {
                v_colorNotes.Add(Mirror_Vertical_Note(colorNote, flip_rows));
            }

            // Obstacles:
            List<Wall> v_obstacleDatas = new List<Wall>();
            if (remove_walls == false)
            {
                foreach (Wall obstacleData in beatmapSaveData.Walls)
                {
                    v_obstacleDatas.Add(Mirror_Vertical_Obstacle(obstacleData, flip_rows));
                }
            }

            // Sliders:
            List<Arc> v_sliderDatas = new List<Arc>();
            foreach (Arc sliderData in beatmapSaveData.Arcs)
            {
                v_sliderDatas.Add(Mirror_Vertical_Slider(sliderData, flip_rows));
            }

            // BurstSliders:
            List<Chain> v_burstSliderDatas = new List<Chain>();
            foreach (Chain burstSliderData in beatmapSaveData.Chains)
            {
                int headcutDirection = burstSliderData.CutDirection;
                var mirroredNote = Mirror_Vertical_BurstSlider(burstSliderData, flip_rows);
                if (mirroredNote.CutDirection != headcutDirection) {
                    foreach (var note in v_colorNotes)
                    {
                        if (mirroredNote.x == note.x && mirroredNote.y == note.y && mirroredNote.Beats == note.Beats) {
                            note.y = mirroredNote.ty;
                        }
                    }

                    var tailLineLayer = mirroredNote.ty;
			        mirroredNote.ty = mirroredNote.y;
			        mirroredNote.y = tailLineLayer;
                }
                v_burstSliderDatas.Add(mirroredNote);
            }


            return new DifficultyV3 {
                Version = beatmapSaveData.Version,
                bpmEvents = beatmapSaveData.bpmEvents,
                Rotations = beatmapSaveData.Rotations,
                Notes = v_colorNotes,
                Bombs = v_bombNotes,
                Walls = v_obstacleDatas,
                Arcs = v_sliderDatas,
                Chains = v_burstSliderDatas,
                Waypoints = beatmapSaveData.Waypoints,
                Lights = beatmapSaveData.Lights,
                colorBoostBeatmapEvents = beatmapSaveData.colorBoostBeatmapEvents,
                lightColorEventBoxGroups = beatmapSaveData.lightColorEventBoxGroups,
                lightRotationEventBoxGroups = beatmapSaveData.lightRotationEventBoxGroups,
                lightTranslationEventBoxGroups = beatmapSaveData.lightTranslationEventBoxGroups,
                vfxEventBoxGroups = beatmapSaveData.vfxEventBoxGroups,
                _fxEventsCollection = beatmapSaveData._fxEventsCollection,
                basicEventTypesWithKeywords = beatmapSaveData.basicEventTypesWithKeywords,
                useNormalEventsAsCompatibleEvents = beatmapSaveData.useNormalEventsAsCompatibleEvents
            };
        }


        public static DifficultyV3 Mirror_Inverse(DifficultyV3 beatmapSaveData, int numberOfLines, bool flip_lines, bool flip_rows, bool remove_walls)
        {
            return Mirror_Vertical(Mirror_Horizontal(beatmapSaveData, numberOfLines, flip_lines, remove_walls), flip_rows, remove_walls);
        }
        #endregion


        #region "Horizontal Transform Functions"

        private static Note Mirror_Horizontal_Note(Note colorNoteData, int numberOfLines, bool flip_lines)
        {
            // Apply Mapping Extensions precision horizontal flip logic to x, and transform cut direction accordingly.
            int mirroredX = colorNoteData.x;
            if (flip_lines) {
                if (mirroredX <= -1000 || mirroredX >= 1000) {
                    var leftSide = false;
                    if (mirroredX <= -1000) mirroredX += 2000;
                    if (mirroredX >= 4000) leftSide = true;
                    mirroredX = 5000 - mirroredX;
                    if (leftSide) mirroredX -= 2000;
                } else if (mirroredX > 3) {
                    var diff = (mirroredX - 3) * 2;
                    var newLaneCount = 4 + diff;
                    mirroredX = newLaneCount - diff - 1 - mirroredX;
                } else if (mirroredX < 0) {
                    var diff = (0 - mirroredX) * 2;
                    var newLaneCount = 4 + diff;
                    mirroredX = newLaneCount - diff - 1 - mirroredX;
                } else {
                    mirroredX = numberOfLines - 1 - mirroredX;
                }
            }

            GridObjectCustomData? customData = colorNoteData.customData;

            if (colorNoteData.customData != null && colorNoteData.customData.coordinates != null) {
                customData = new GridObjectCustomData {
                    coordinates = new float[] { -colorNoteData.customData.coordinates[0] - 1, colorNoteData.customData.coordinates[1] },
                };
            }

            var newCut = horizontal_cut_transform.TryGetValue((Note.Direction)colorNoteData.CutDirection, out var mapped)
                ? mapped
                : (Note.Direction)colorNoteData.CutDirection;

            return new Note {
                Beats = colorNoteData.Beats,
                Seconds = colorNoteData.Seconds,
                BpmTime = colorNoteData.BpmTime,
                x = mirroredX,
                y = colorNoteData.y,
                Color = colorNoteData.Color,
                CutDirection = (int)newCut,
                AngleOffset = -colorNoteData.AngleOffset,
                customData = customData
            };
        }

        private static Bomb Mirror_Horizontal_Bomb(Bomb bombData, int numberOfLines, bool flip_lines)
        {
            int mirrorX(int value) {
                var v = value;
                if (!flip_lines) return v;
                if (v <= -1000 || v >= 1000) {
                    var leftSide = false;
                    if (v <= -1000) v += 2000;
                    if (v >= 4000) leftSide = true;
                    v = 5000 - v;
                    if (leftSide) v -= 2000;
                } else if (v > 3) {
                    var diff = (v - 3) * 2;
                    var newLaneCount = 4 + diff;
                    v = newLaneCount - diff - 1 - v;
                } else if (v < 0) {
                    var diff = (0 - v) * 2;
                    var newLaneCount = 4 + diff;
                    v = newLaneCount - diff - 1 - v;
                } else {
                    v = numberOfLines - 1 - v;
                }
                return v;
            }

            GridObjectCustomData? customData = bombData.customData;

            if (bombData.customData != null && bombData.customData.coordinates != null) {
                customData = new GridObjectCustomData {
                    coordinates = new float[] { -bombData.customData.coordinates[0] - 1, bombData.customData.coordinates[1] },
                };
            }

            var mirroredX = mirrorX(bombData.x);
            return new Bomb {
                Beats = bombData.Beats,
                Seconds = bombData.Seconds,
                BpmTime = bombData.BpmTime,
                x = mirroredX,
                y = bombData.y,
                customData = customData
            };
        }


        private static Wall Mirror_Horizontal_Obstacle(Wall obstacleData, int numberOfLines, bool flip_lines)
        {
            if (!flip_lines) {
                return obstacleData;
            }

            var lineIndex = obstacleData.x;
            var obstacleWidth = obstacleData.Width;
            var precisionWidth = obstacleWidth >= 1000 || obstacleWidth <= -1000;

            if (lineIndex <= 3 && lineIndex >= 0 && !precisionWidth)
            {
                var mirrorLane = (lineIndex - 2) * -1 + 2;
                return new Wall {
                    Beats = obstacleData.Beats,
                    Seconds = obstacleData.Seconds,
                    BpmTime = obstacleData.BpmTime,
                    x = mirrorLane - obstacleWidth,
                    y = obstacleData.y,
                    DurationInBeats = obstacleData.DurationInBeats,
                    Width = obstacleData.Width,
                    Height = obstacleData.Height
                };
            }

            // Precision logic adapted from MappingExtensions ObstacleData.Mirror
            int idx;
            if (lineIndex <= -1000)
            {
                idx = lineIndex + 1000; // normalize
            }
            else if (lineIndex >= 1000)
            {
                idx = lineIndex - 1000; // normalize
            }
            else
            {
                idx = lineIndex * 1000; // convert to precision
            }

            idx = (idx - 2000) * -1 + 2000; // flip

            int width;
            if (obstacleWidth < 1000 && obstacleWidth > -1000)
            {
                width = obstacleWidth * 1000; // normalize width
            }
            else
            {
                width = obstacleWidth;
                if (width >= 1000) width -= 1000;
                if (width <= -1000) width += 1000;
            }

            idx -= width;
            if (idx < 0) idx -= 1000; else idx += 1000; // fix bounds

            return new Wall {
                Beats = obstacleData.Beats,
                Seconds = obstacleData.Seconds,
                BpmTime = obstacleData.BpmTime,
                x = idx,
                y = obstacleData.y,
                DurationInBeats = obstacleData.DurationInBeats,
                Width = obstacleData.Width,
                Height = obstacleData.Height
            };
        }

        private static Arc Mirror_Horizontal_Slider(Arc sliderData, int numberOfLines, bool flip_lines)
        {
            int mirrorX(int value) {
                var v = value;
                if (!flip_lines) return v;
                if (v <= -1000 || v >= 1000) {
                    var leftSide = false;
                    if (v <= -1000) v += 2000;
                    if (v >= 4000) leftSide = true;
                    v = 5000 - v;
                    if (leftSide) v -= 2000;
                } else if (v > 3) {
                    var diff = (v - 3) * 2;
                    var newLaneCount = 4 + diff;
                    v = newLaneCount - diff - 1 - v;
                } else if (v < 0) {
                    var diff = (0 - v) * 2;
                    var newLaneCount = 4 + diff;
                    v = newLaneCount - diff - 1 - v;
                } else {
                    v = numberOfLines - 1 - v;
                }
                return v;
            }

            var headX = mirrorX(sliderData.x);
            var tailX = mirrorX(sliderData.tx);
            var headCut = horizontal_cut_transform.TryGetValue((Note.Direction)sliderData.CutDirection, out var hHead) ? hHead : (Note.Direction)sliderData.CutDirection;
            var tailCut = horizontal_cut_transform.TryGetValue((Note.Direction)sliderData.TailCutDirection, out var hTail) ? hTail : (Note.Direction)sliderData.TailCutDirection;

            return new Arc {
                Color = sliderData.Color,
                Beats = sliderData.Beats,
                Seconds = sliderData.Seconds,
                BpmTime = sliderData.BpmTime,
                x = headX,
                y = sliderData.y,
                CutDirection = (int)headCut,
                TailInBeats = sliderData.TailInBeats,
                tx = tailX,
                ty = sliderData.ty,
                TailCutDirection = (int)tailCut,
                Multiplier = sliderData.Multiplier,
                TailMultiplier = sliderData.TailMultiplier,
                AnchorMode = sliderData.AnchorMode,
                customData = sliderData.customData
            };
        }


        private static Chain Mirror_Horizontal_BurstSlider(Chain burstSliderData, int numberOfLines, bool flip_lines)
        {
            int mirrorX(int value) {
                var v = value;
                if (!flip_lines) return v;
                if (v <= -1000 || v >= 1000) {
                    var leftSide = false;
                    if (v <= -1000) v += 2000;
                    if (v >= 4000) leftSide = true;
                    v = 5000 - v;
                    if (leftSide) v -= 2000;
                } else if (v > 3) {
                    var diff = (v - 3) * 2;
                    var newLaneCount = 4 + diff;
                    v = newLaneCount - diff - 1 - v;
                } else if (v < 0) {
                    var diff = (0 - v) * 2;
                    var newLaneCount = 4 + diff;
                    v = newLaneCount - diff - 1 - v;
                } else {
                    v = numberOfLines - 1 - v;
                }
                return v;
            }

            var headX = mirrorX(burstSliderData.x);
            var tailX = mirrorX(burstSliderData.tx);
            var headCut = horizontal_cut_transform.TryGetValue((Note.Direction)burstSliderData.CutDirection, out var hHead) ? hHead : (Note.Direction)burstSliderData.CutDirection;

            return new Chain {
                Color = burstSliderData.Color,
                Beats = burstSliderData.Beats,
                Seconds = burstSliderData.Seconds,
                BpmTime = burstSliderData.BpmTime,
                x = headX,
                y = burstSliderData.y,
                CutDirection = (int)headCut,
                TailInBeats = burstSliderData.TailInBeats,
                tx = tailX,
                ty = burstSliderData.ty,
                SliceCount = burstSliderData.SliceCount,
                Squish = burstSliderData.Squish
            };
        }
        #endregion


        #region "Vertical Transform Functions"


        private static Note Mirror_Vertical_Note(Note colorNoteData, bool flip_rows)
        {
            // Apply Mapping Extensions precision vertical flip logic to y, and transform cut direction accordingly.
            int mirroredY = colorNoteData.y;
            if (flip_rows) {
                if (mirroredY <= -1000 || mirroredY >= 1000) {
                    // Vertical precision behaves like horizontal but on rows: flip around center 2*1000 and reapply side offset when needed.
                    bool bottomSide = false;
                    if (mirroredY <= -1000) mirroredY += 2000;
                    if (mirroredY >= 4000) bottomSide = true;
                    mirroredY = 5000 - mirroredY;
                    if (bottomSide) mirroredY -= 2000;
                } else if (mirroredY > 2) { // rows are 0..2 standard
                    var diff = (mirroredY - 2) * 2;
                    var newRowCount = 3 + diff;
                    mirroredY = newRowCount - diff - 1 - mirroredY;
                } else if (mirroredY < 0) {
                    var diff = (0 - mirroredY) * 2;
                    var newRowCount = 3 + diff;
                    mirroredY = newRowCount - diff - 1 - mirroredY;
                } else {
                    mirroredY = 3 - 1 - mirroredY;
                }
            }

            var newCut = vertical_cut_transform.TryGetValue((Note.Direction)colorNoteData.CutDirection, out var mapped)
                ? mapped
                : (Note.Direction)colorNoteData.CutDirection;

            return new Note {
                Beats = colorNoteData.Beats,
                Seconds = colorNoteData.Seconds,
                BpmTime = colorNoteData.BpmTime,
                x = colorNoteData.x,
                y = mirroredY,
                Color = colorNoteData.Color,
                CutDirection = (int)newCut,
                AngleOffset = colorNoteData.AngleOffset
            };
        }

        private static Bomb Mirror_Vertical_Bomb(Bomb bombData, bool flip_rows)
        {
            int mirrorY(int value) {
                var v = value;
                if (!flip_rows) return v;
                if (v <= -1000 || v >= 1000) {
                    bool bottomSide = false;
                    if (v <= -1000) v += 2000;
                    if (v >= 4000) bottomSide = true;
                    v = 5000 - v;
                    if (bottomSide) v -= 2000;
                } else if (v > 2) { // standard rows 0..2
                    var diff = (v - 2) * 2;
                    var newRowCount = 3 + diff;
                    v = newRowCount - diff - 1 - v;
                } else if (v < 0) {
                    var diff = (0 - v) * 2;
                    var newRowCount = 3 + diff;
                    v = newRowCount - diff - 1 - v;
                } else {
                    v = 3 - 1 - v;
                }
                return v;
            }

            var mirroredY = mirrorY(bombData.y);
            return new Bomb {
                Beats = bombData.Beats,
                Seconds = bombData.Seconds,
                BpmTime = bombData.BpmTime,
                x = bombData.x,
                y = mirroredY,
                customData = bombData.customData
            };
        }


        private static Wall Mirror_Vertical_Obstacle(Wall obstacleData, bool flip_rows)
        {
            // Vertical flip does not change obstacle when removing walls is false; ME does not define vertical precision for obstacles here.
            return obstacleData;
        }

        private static Arc Mirror_Vertical_Slider(Arc sliderData, bool flip_rows)
        {
            int mirrorY(int value) {
                var v = value;
                if (!flip_rows) return v;
                if (v <= -1000 || v >= 1000) {
                    bool bottomSide = false;
                    if (v <= -1000) v += 2000;
                    if (v >= 4000) bottomSide = true;
                    v = 5000 - v;
                    if (bottomSide) v -= 2000;
                } else if (v > 2) {
                    var diff = (v - 2) * 2;
                    var newRowCount = 3 + diff;
                    v = newRowCount - diff - 1 - v;
                } else if (v < 0) {
                    var diff = (0 - v) * 2;
                    var newRowCount = 3 + diff;
                    v = newRowCount - diff - 1 - v;
                } else {
                    v = 3 - 1 - v;
                }
                return v;
            }

            var headY = mirrorY(sliderData.y);
            var tailY = mirrorY(sliderData.ty);
            var headCut = vertical_cut_transform.TryGetValue((Note.Direction)sliderData.CutDirection, out var vHead) ? vHead : (Note.Direction)sliderData.CutDirection;
            var tailCut = vertical_cut_transform.TryGetValue((Note.Direction)sliderData.TailCutDirection, out var vTail) ? vTail : (Note.Direction)sliderData.TailCutDirection;

            return new Arc {
                Color = sliderData.Color,
                Beats = sliderData.Beats,
                Seconds = sliderData.Seconds,
                BpmTime = sliderData.BpmTime,
                x = sliderData.x,
                y = headY,
                CutDirection = (int)headCut,
                TailInBeats = sliderData.TailInBeats,
                tx = sliderData.tx,
                ty = tailY,
                TailCutDirection = (int)tailCut,
                Multiplier = sliderData.Multiplier,
                TailMultiplier = sliderData.TailMultiplier,
                AnchorMode = sliderData.AnchorMode,
                customData = sliderData.customData
            };
        }


        private static Chain Mirror_Vertical_BurstSlider(Chain burstSliderData, bool flip_rows)
        {
            int mirrorY(int value) {
                var v = value;
                if (!flip_rows) return v;
                if (v <= -1000 || v >= 1000) {
                    bool bottomSide = false;
                    if (v <= -1000) v += 2000;
                    if (v >= 4000) bottomSide = true;
                    v = 5000 - v;
                    if (bottomSide) v -= 2000;
                } else if (v > 2) {
                    var diff = (v - 2) * 2;
                    var newRowCount = 3 + diff;
                    v = newRowCount - diff - 1 - v;
                } else if (v < 0) {
                    var diff = (0 - v) * 2;
                    var newRowCount = 3 + diff;
                    v = newRowCount - diff - 1 - v;
                } else {
                    v = 3 - 1 - v;
                }
                return v;
            }

            var headY = mirrorY(burstSliderData.y);
            var tailY = mirrorY(burstSliderData.ty);
            var headCut = vertical_cut_transform.TryGetValue((Note.Direction)burstSliderData.CutDirection, out var vHead) ? vHead : (Note.Direction)burstSliderData.CutDirection;

            return new Chain {
                Color = burstSliderData.Color,
                Beats = burstSliderData.Beats,
                Seconds = burstSliderData.Seconds,
                BpmTime = burstSliderData.BpmTime,
                x = burstSliderData.x,
                y = headY,
                CutDirection = (int)headCut,
                TailInBeats = burstSliderData.TailInBeats,
                tx = burstSliderData.tx,
                ty = tailY,
                SliceCount = burstSliderData.SliceCount,
                Squish = burstSliderData.Squish
            };
        }

        #endregion
    }
}
