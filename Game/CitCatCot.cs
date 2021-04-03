using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public sealed class CitCatCot
    {
        private bool finished;
        private bool firstMove;
        private readonly bool?[][] board;
        
        public CitCatCot(object first)
        {
            First = first;
            finished = false;
            firstMove = true;
            board = new bool?[5][];
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = new bool?[5];
                for (int j = 0; j < board[i].Length; j++)
                {
                    board[i][j] = null;
                }
            }
        }
        
        public object First { get; }

        public bool Moves(object obj)
        {
            if (finished)
            {
                return false;
            }

            if (firstMove)
            {
                return ReferenceEquals(obj, First);
            }

            return !ReferenceEquals(obj, First);
        }

        public CitCatCotActionResult Move(int i, int j)
        {
            if (finished)
            {
                return CitCatCotActionResult.Fail;
            }

            if (board[i][j] != null)
            {
                return CitCatCotActionResult.Fail;
            }

            board[i][j] = firstMove;
            var result = GetStepState(i, j);
            if (result != CitCatCotActionResult.Success)
            {
                finished = true;
            }

            firstMove = !firstMove;
            return result;
        }

        private CitCatCotActionResult GetStepState(int i, int j)
        {
            var triplets = GetTriplets(i, j);

            foreach (var triplet in triplets)
            {
                var first = board[triplet[0]][triplet[1]];
                var second = board[triplet[2]][triplet[3]];
                var third = board[triplet[4]][triplet[5]];

                if (first == firstMove && second == firstMove && third == firstMove)
                {
                    return firstMove ? CitCatCotActionResult.WinSecond : CitCatCotActionResult.WinFirst;
                }
            }

            if (board.All(cs => cs.All(c => c != null)))
            {
                return CitCatCotActionResult.Draw;
            }

            return CitCatCotActionResult.Success;
        }

        private int[][] GetTriplets(int i, int j)
        {
            var result = new List<int[]>(12);

            int i_1 = i - 1;
            int i_2 = i - 2;
            int i1 = i + 1;
            int i2 = i + 2;
            int j_1 = j - 1;
            int j_2 = j - 2;
            int j1 = j + 1;
            int j2 = j + 2;
            
            bool _i_1 = i_1 >= 0;
            bool _i_2 = i_2 >= 0;
            bool _i1 = i1 < 5;
            bool _i2 = i2 < 5;
            bool _j_1 = j_1 >= 0;
            bool _j_2 = j_2 >= 0;
            bool _j1 = j1 < 5;
            bool _j2 = j2 < 5;
            
            // centers
            // diag
            if (_i_1 && _j_1 && _i1 && _j1) result.Add(new[] {i_1, j_1, i, j, i1, j1});
            if (_i_1 && _j1 && _i1 && _j_1) result.Add(new[] {i_1, j1, i, j, i1, j_1});
            // str
            if (_j_1 && _j1) result.Add(new[] {i, j_1, i, j, i, j1});
            if (_i_1 && _i1) result.Add(new[] {i_1, j, i, j, i1, j});
            
            // longs
            // diag
            if (_i_1 && _j_1 && _i_2 && _j_2) result.Add(new[] {i_2, j_2, i_1, j_1, i, j});
            if (_i_1 && _j1 && _i_2 && _j2) result.Add(new[] {i_2, j2, i_1, j1, i, j});
            if (_i1 && _j_1 && _i2 && _j_2) result.Add(new[] {i2, j_2, i1, j_1, i, j});
            if (_i1 && _j1 && _i2 && _j2) result.Add(new[] {i2, j2, i1, j1, i, j});
            // str
            if (_j_2 && _j_1) result.Add(new[] {i, j_2, i, j_1, i, j});
            if (_j2 && _j1) result.Add(new[] {i, j2, i, j1, i, j});
            if (_i_2 && _i_1) result.Add(new[] {i_2, j, i_1, j, i, j});
            if (_i2 && _i1) result.Add(new[] {i2, j, i1, j, i, j});

            return result.ToArray();
        }
    }
}