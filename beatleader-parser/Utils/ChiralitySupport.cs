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
        public static DifficultyV3 Mirror_Horizontal(DifficultyV3 beatmapSaveData, int numberOfLines, bool flip_lines, bool remove_walls, bool is_ME)
        {
            // Bombs:
            List<Bomb> h_bombNotes = new List<Bomb>();
            foreach (Bomb bomb in beatmapSaveData.Bombs)
            {
                if (flip_lines == false)
                {
                    h_bombNotes.Add(new Bomb { 
                       Beats = bomb.Beats, 
                       x = bomb.x, 
                       y = bomb.y 
                    });
                }
                else
                {
                    h_bombNotes.Add(new Bomb { Beats = bomb.Beats, x = numberOfLines - 1 - bomb.x, y = bomb.y });
                }
            }

            // ColorNotes:
            List<Note> h_colorNotes = new List<Note>();
            foreach (Note colorNote in beatmapSaveData.Notes)
            {
                h_colorNotes.Add(Mirror_Horizontal_Note(colorNote, numberOfLines, flip_lines, is_ME));
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
                h_sliderDatas.Add(Mirror_Horizontal_Slider(sliderData, numberOfLines, flip_lines, is_ME));
            }

            // BurstSliders:
            List<Chain> h_burstSliderDatas = new List<Chain>();
            foreach (Chain burstSliderData in beatmapSaveData.Chains)
            {
                int headcutDirection = burstSliderData.CutDirection;
                var mirroredNote = Mirror_Horizontal_BurstSlider(burstSliderData, numberOfLines, flip_lines, is_ME);
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


        public static DifficultyV3 Mirror_Vertical(DifficultyV3 beatmapSaveData, bool flip_rows, bool remove_walls, bool is_ME)
        {
            // Bombs:
            List<Bomb> v_bombNotes = new List<Bomb>();
            foreach (Bomb bomb in beatmapSaveData.Bombs)
            {
                if (flip_rows)
                {
                    v_bombNotes.Add(new Bomb { Beats = bomb.Beats, x = bomb.x, y = 3 - 1 - bomb.y });
                }
                else
                {
                    v_bombNotes.Add(bomb);
                }
            }

            // ColorNotes:
            List<Note> v_colorNotes = new List<Note>();
            foreach (Note colorNote in beatmapSaveData.Notes)
            {
                v_colorNotes.Add(Mirror_Vertical_Note(colorNote, flip_rows, is_ME));
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
                v_sliderDatas.Add(Mirror_Vertical_Slider(sliderData, flip_rows, is_ME));
            }

            // BurstSliders:
            List<Chain> v_burstSliderDatas = new List<Chain>();
            foreach (Chain burstSliderData in beatmapSaveData.Chains)
            {
                int headcutDirection = burstSliderData.CutDirection;
                var mirroredNote = Mirror_Vertical_BurstSlider(burstSliderData, flip_rows, is_ME);
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


        public static DifficultyV3 Mirror_Inverse(DifficultyV3 beatmapSaveData, int numberOfLines, bool flip_lines, bool flip_rows, bool remove_walls, bool is_ME)
        {
            return Mirror_Vertical(Mirror_Horizontal(beatmapSaveData, numberOfLines, flip_lines, remove_walls, is_ME), flip_rows, remove_walls, is_ME);
        }
        #endregion


        #region "Horizontal Transform Functions"

        private static Note Mirror_Horizontal_Note(Note colorNoteData, int numberOfLines, bool flip_lines, bool is_ME)
        {
            int h_line;

            int color;
            if (colorNoteData.Color == (int)Note.Type.Red)
            {
                color = (int)Note.Type.Blue;
            }
            else
            {
                color = (int)Note.Type.Red;
            }

            if (colorNoteData.x >= 1000 || colorNoteData.x <= -1000)
            {
                h_line = colorNoteData.x / 1000 - 1;
                color = colorNoteData.Color;
            }
            else if (flip_lines)
            {
                h_line = numberOfLines - 1 - colorNoteData.x;
            }
            else
            {
                h_line = colorNoteData.x;
                color = colorNoteData.Color;
            }

            Note.Direction h_cutDirection;
            if (horizontal_cut_transform.TryGetValue((Note.Direction)colorNoteData.CutDirection, out h_cutDirection) == false || is_ME)
            {
                h_cutDirection = Get_Random_Direction();
            }

            return new Note { 
                Beats = colorNoteData.Beats,
                Seconds = colorNoteData.Seconds,
                BpmTime = colorNoteData.BpmTime,
                x = h_line,
                y = Check_Layer(colorNoteData.y),
                Color = color,
                CutDirection = (int)h_cutDirection,
                AngleOffset = colorNoteData.AngleOffset
            };
        }


        private static Wall Mirror_Horizontal_Obstacle(Wall obstacleData, int numberOfLines, bool flip_lines)
        {
            if (flip_lines)
            {
                return new Wall {
                    Beats = obstacleData.Beats,
                    Seconds = obstacleData.Seconds,
                    BpmTime = obstacleData.BpmTime,
                    x = numberOfLines - obstacleData.Width - obstacleData.x,
                    y = obstacleData.y,
                    DurationInBeats = obstacleData.DurationInBeats,
                    Width = obstacleData.Width,
                    Height = obstacleData.Height
                };
            }

            return obstacleData;
        }

        private static Arc Mirror_Horizontal_Slider(Arc sliderData, int numberOfLines, bool flip_lines, bool is_ME)
        {
            int h_headline;
            int h_tailline;

            int color;
            if (sliderData.Color == (int)Note.Type.Red)
            {
                color = (int)Note.Type.Blue;
            }
            else
            {
                color = (int)Note.Type.Red;
            }


            if (sliderData.x >= 1000 || sliderData.x <= -1000)
            {
                h_headline = sliderData.x / 1000 - 1;
                color = sliderData.Color;
            }
            else if (flip_lines)
            {
                h_headline = numberOfLines - 1 - sliderData.x;
            }
            else
            {
                h_headline = sliderData.x;
                color = sliderData.Color;
            }


            if (sliderData.tx >= 1000 || sliderData.tx <= -1000)
            {
                h_tailline = sliderData.tx / 1000 - 1;
                color = sliderData.Color;
            }
            else if (flip_lines)
            {
                h_tailline = numberOfLines - 1 - sliderData.tx;
            }
            else
            {
                h_tailline = sliderData.tx;
                color = sliderData.Color;
            }

            Note.Direction h_headcutDirection;
            if (horizontal_cut_transform.TryGetValue((Note.Direction)sliderData.CutDirection, out h_headcutDirection) == false || is_ME)
            {
                h_headcutDirection = Get_Random_Direction();
            }

            Note.Direction h_tailcutDirection;
            if (horizontal_cut_transform.TryGetValue((Note.Direction)sliderData.CutDirection, out h_tailcutDirection) == false || is_ME)
            {
                h_headcutDirection = Get_Random_Direction();
            }

            return new Arc {
                Color = color,
                Beats = sliderData.Beats,
                Seconds = sliderData.Seconds,
                BpmTime = sliderData.BpmTime,
                x = h_headline,
                y = Check_Layer(sliderData.y),
                CutDirection = (int)h_headcutDirection,
                TailInBeats = sliderData.TailInBeats,
                tx = h_tailline,
                ty = Check_Layer(sliderData.ty)
            };
        }


        private static Chain Mirror_Horizontal_BurstSlider(Chain burstSliderData, int numberOfLines, bool flip_lines, bool is_ME)
        {
            int h_headline;
            int h_tailline;

            int color;
            if (burstSliderData.Color == (int)Note.Type.Red)
            {
                color = (int)Note.Type.Blue;
            }
            else
            {
                color = (int)Note.Type.Red;
            }


            if (burstSliderData.x >= 1000 || burstSliderData.x <= -1000)
            {
                h_headline = burstSliderData.x / 1000 - 1;
                color = burstSliderData.Color;
            }
            else if (flip_lines)
            {
                h_headline = numberOfLines - 1 - burstSliderData.x;
            }
            else
            {
                h_headline = burstSliderData.x;
                color = burstSliderData.Color;
            }


            if (burstSliderData.tx >= 1000 || burstSliderData.tx <= -1000)
            {
                h_tailline = burstSliderData.tx / 1000 - 1;
                color = burstSliderData.Color;
            }
            else if (flip_lines)
            {
                h_tailline = numberOfLines - 1 - burstSliderData.tx;
            }
            else
            {
                h_tailline = burstSliderData.tx;
                color = burstSliderData.Color;
            }

            Note.Direction h_headcutDirection;
            if (horizontal_cut_transform.TryGetValue((Note.Direction)burstSliderData.CutDirection, out h_headcutDirection) == false || is_ME)
            {
                h_headcutDirection = Get_Random_Direction();
            }

            return new Chain {
                Color = color,
                Beats = burstSliderData.Beats,
                Seconds = burstSliderData.Seconds,
                BpmTime = burstSliderData.BpmTime,
                x = h_headline,
                y = Check_Layer(burstSliderData.y),
                CutDirection = (int)h_headcutDirection,
                TailInBeats = burstSliderData.TailInBeats,
                tx = h_tailline,
                ty = Check_Layer(burstSliderData.ty),
                SliceCount = burstSliderData.SliceCount,
                Squish = burstSliderData.Squish
            };
        }
        #endregion


        #region "Vertical Transform Functions"


        private static Note Mirror_Vertical_Note(Note colorNoteData, bool flip_rows, bool has_ME)
        {
            int v_layer;

            if (colorNoteData.y >= 1000 || colorNoteData.y <= -1000)
            {
                v_layer = (colorNoteData.y / 1000) - 1;
            }
            else if (flip_rows)
            {
                v_layer = 3 - 1 - colorNoteData.y;
            }
            else
            {
                v_layer = colorNoteData.y;
            }

            Note.Direction v_cutDirection;
            if (vertical_cut_transform.TryGetValue((Note.Direction)colorNoteData.CutDirection, out v_cutDirection) == false || has_ME)
            {
                v_cutDirection = Get_Random_Direction();
            }

            return new Note {
                Beats = colorNoteData.Beats,
                Seconds = colorNoteData.Seconds,
                BpmTime = colorNoteData.BpmTime,
                x = Check_Index(colorNoteData.x),
                y = v_layer,
                Color = colorNoteData.Color,
                CutDirection = (int)v_cutDirection,
                AngleOffset = colorNoteData.AngleOffset
            };
        }


        private static Wall Mirror_Vertical_Obstacle(Wall obstacleData, bool flip_rows)
        {
            if (flip_rows)
            {
                return new Wall {
                    Beats = obstacleData.Beats,
                    Seconds = obstacleData.Seconds,
                    BpmTime = obstacleData.BpmTime,
                    x = 0,
                    y = 0,
                    DurationInBeats = 0,
                    Width = 0,
                    Height = 0
                };
            }

            return obstacleData;
        }

        private static Arc Mirror_Vertical_Slider(Arc sliderData, bool flip_rows, bool has_ME)
        {
            int v_headlayer;
            int v_taillayer;

            if (sliderData.y >= 1000 || sliderData.y <= -1000)
            {
                v_headlayer = (sliderData.y / 1000) - 1;
            }
            else if (flip_rows)
            {
                v_headlayer = 3 - 1 - sliderData.y;
            }
            else
            {
                v_headlayer = sliderData.y;
            }


            if (sliderData.ty >= 1000 || sliderData.ty <= -1000)
            {
                v_taillayer = (sliderData.ty / 1000) - 1;
            }
            else if (flip_rows)
            {
                v_taillayer = 3 - 1 - sliderData.ty;
            }
            else
            {
                v_taillayer = sliderData.ty;
            }


            Note.Direction v_headcutDirection;
            if (vertical_cut_transform.TryGetValue((Note.Direction)sliderData.CutDirection, out v_headcutDirection) == false || has_ME)
            {
                v_headcutDirection = Get_Random_Direction();
            }

            Note.Direction v_tailcutDirection;
            if (vertical_cut_transform.TryGetValue((Note.Direction)sliderData.CutDirection, out v_tailcutDirection) == false || has_ME)
            {
                v_tailcutDirection = Get_Random_Direction();
            }


            return new Arc {
                Color = sliderData.Color,
                Beats = sliderData.Beats,
                Seconds = sliderData.Seconds,
                BpmTime = sliderData.BpmTime,
                x = Check_Index(sliderData.x),
                y = v_headlayer,
                CutDirection = (int)v_headcutDirection,
                TailInBeats = sliderData.TailInBeats,
                tx = Check_Index(sliderData.tx),
                ty = v_taillayer,
                TailCutDirection = (int)v_tailcutDirection
            };
        }


        private static Chain Mirror_Vertical_BurstSlider(Chain burstSliderData, bool flip_rows, bool has_ME)
        {
            int v_headlayer;
            int v_taillayer;

            if (burstSliderData.y >= 1000 || burstSliderData.y <= -1000)
            {
                v_headlayer = (burstSliderData.y / 1000) - 1;
            }
            else if (flip_rows)
            {
                v_headlayer = 3 - 1 - burstSliderData.y;
            }
            else
            {
                v_headlayer = burstSliderData.y;
            }


            if (burstSliderData.ty >= 1000 || burstSliderData.ty <= -1000)
            {
                v_taillayer = (burstSliderData.ty / 1000) - 1;
            }
            else if (flip_rows)
            {
                v_taillayer = 3 - 1 - burstSliderData.ty;
            }
            else
            {
                v_taillayer = burstSliderData.ty;
            }


            Note.Direction v_headcutDirection;
            if (vertical_cut_transform.TryGetValue((Note.Direction)burstSliderData.CutDirection, out v_headcutDirection) == false || has_ME)
            {
                v_headcutDirection = Get_Random_Direction();
            }

            return new Chain {
                Color = burstSliderData.Color,
                Beats = burstSliderData.Beats,
                Seconds = burstSliderData.Seconds,
                BpmTime = burstSliderData.BpmTime,
                x = Check_Index(burstSliderData.x),
                y = v_headlayer,
                CutDirection = (int)v_headcutDirection,
                TailInBeats = burstSliderData.TailInBeats,
                tx = Check_Index(burstSliderData.tx),
                ty = v_taillayer,
                SliceCount = burstSliderData.SliceCount,
                Squish = burstSliderData.Squish
            };
        }

        #endregion


        #region "Utility Functions"
        public static Note.Direction Get_Random_Direction()
        {
            int index = rand.Next(directions.Count);

            return directions[index];
        }

        public static int Check_Index(int lineIndex)
        {
            if (lineIndex >= 500 || lineIndex <= -500)
            {
                return lineIndex / 1000;
            }

            return lineIndex;
        }

        public static int Check_Layer(int lineLayer)
        {
            if (lineLayer >= 500 || lineLayer <= -500)
            {
                return lineLayer / 1000;
            }

            return lineLayer;
        }
        #endregion
    }
}
