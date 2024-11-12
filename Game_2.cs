using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_1
{
    class Game_2
    {



        public class Game
        {
            public int BorderLeftX = 0;
            public int BorderRightX = 9;
            public int BorderTopY = 0;
            public int BorderBottomY = 9;

            List<Grass> grasses = new();
            List<Rock> rocks = new();
            List<Tree> treeses = new();
            Character player = new(5, 5);
            List<Plate> plates = new();
            List<Cell> cells = new();


            public Game()
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        grasses.Add(new Grass(x, y));
                    }
                }

                rocks.Add(new Rock(5, 6));
                treeses.Add(new Tree(7, 7));
                plates.Add(new Plate(2, 2));
                plates.Add(new Plate(2, 3));

                cells.AddRange(grasses);
                cells.AddRange(rocks);
                cells.AddRange(treeses);
                cells.Add(player);
                cells.AddRange(plates);
            }

            public void Print()
            {
                var field = new string[10, 10];

                foreach (var grass in grasses) field[grass.X, grass.Y] = grass.Value;
                foreach (var rock in rocks) field[rock.X, rock.Y] = rock.Value;
                foreach (var tree in treeses) field[tree.X, tree.Y] = tree.Value;

                foreach (var plate in plates)
                {
                    field[plate.X, plate.Y] = plate.ToString();
                }


                Console.ForegroundColor = ConsoleColor.Red;


                foreach (var plate in plates)
                {
                    if (player.X == plate.X && player.Y == plate.Y && plate.IsActive)
                    {
                        field[plate.X, plate.Y] = "₽";
                    }
                }


                if (field[player.X, player.Y] != "₽")
                {
                    field[player.X, player.Y] = player.Value;
                }
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {

                        switch (field[y, x])
                        {
                            case "#":
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case "R":
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case "T":
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case "C":
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case "@":
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case "₽":
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                        }

                        Console.Write(field[y, x] + " ");
                    }
                    Console.WriteLine();
                }


                Console.ResetColor();
            }
            public bool CanIMoveCharacter(int x, int y)
            {
                int targetX = player.X + x;
                int targetY = player.Y + y;


                if (targetX < BorderLeftX || targetX > BorderRightX || targetY < BorderTopY || targetY > BorderBottomY)
                {
                    return false;
                }


                foreach (var cell in cells)
                {
                    if (cell.X == targetX && cell.Y == targetY)
                    {
                        if (!cell.IsCrossable)
                        {
                            return false;
                        }

                        if (cell is Rock rock)
                        {

                            int nextX = rock.X + x;
                            int nextY = rock.Y + y;

                            if (!IsPositionFree(nextX, nextY) || !IsWithinBounds(nextX, nextY))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            public bool MoveRockIfPossible(int x, int y)
            {
                int targetX = player.X + x;
                int targetY = player.Y + y;

                foreach (var cell in cells)
                {
                    if (cell is Rock rock && rock.X == targetX && rock.Y == targetY)
                    {
                        int nextX = rock.X + x;
                        int nextY = rock.Y + y;

                        if (IsPositionFree(nextX, nextY) && IsWithinBounds(nextX, nextY))
                        {

                            rock.X = nextX;
                            rock.Y = nextY;


                            ActivatePlateIfUnderRock(rock);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Невозможно переместить камень на это место.");
                        }
                    }
                }

                return false;
            }

            public void ActivatePlateIfUnderRock(Rock rock)
            {
                foreach (var cell in cells)
                {
                    if (cell is Plate plate && plate.X == rock.X && plate.Y == rock.Y)
                    {
                        plate.ActivateByRock();
                    }
                }
            }


            public void ActivatePlateIfUnderCharacter(Character character)
            {
                foreach (var cell in cells)
                {
                    if (cell is Plate plate && plate.X == character.X && plate.Y == character.Y)
                    {
                        plate.ActivateByCharacter();
                    }
                }
            }

            public void DeactivatePlateIfNotOccupied()
            {
                foreach (var cell in cells)
                {
                    if (cell is Plate plate)
                    {

                        if (!IsPlateOccupied(plate))
                        {
                            plate.Deactivate();
                        }
                    }
                }
            }

            public bool IsPlateOccupied(Plate plate)
            {
                foreach (var cell in cells)
                {
                    if ((cell is Rock rock && rock.X == plate.X && rock.Y == plate.Y) ||
                        (cell is Character character && character.X == plate.X && character.Y == plate.Y))
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool IsPositionFree(int x, int y)
            {
                foreach (var cell in cells)
                {
                    if (cell.X == x && cell.Y == y && !cell.IsCrossable)
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool IsWithinBounds(int x, int y)
            {
                return x >= BorderLeftX && x <= BorderRightX && y >= BorderTopY && y <= BorderBottomY;
            }

            public void MoveCharacter(int x, int y)
            {
                if (CanIMoveCharacter(x, y))
                {
                    player.X += x;
                    player.Y += y;


                    ActivatePlateIfUnderCharacter(player);
                    DeactivatePlateIfNotOccupied();


                    if (CheckVictoryCondition())
                    {
                        Console.WriteLine("Поздравляю! Вы победили!");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("Вы не можете переместиться в этом направлении.");
                }
            }

            public bool CheckVictoryCondition()
            {

                int activatedPlatesCount = 0;

                foreach (var plate in plates)
                {
                    if (plate.IsActive)
                    {
                        activatedPlatesCount++;
                    }
                }

                return activatedPlatesCount == plates.Count;
            }
            public void Run()
            {
                while (true)
                {
                    Print();


                    if (IsLevelCompleted())
                    {
                        Console.WriteLine("Вы выиграли! Начинаем новый уровень...");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }

                    var key = Console.ReadKey();

                    int dx = 0;
                    int dy = 0;

                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            dx = -1;
                            break;
                        case ConsoleKey.RightArrow:
                            dx = 1;
                            break;
                        case ConsoleKey.UpArrow:
                            dy = -1;
                            break;
                        case ConsoleKey.DownArrow:
                            dy = 1;
                            break;
                    }

                    if (CanIMoveCharacter(dx, dy))
                    {
                        MoveCharacter(dx, dy);
                    }
                    else if (MoveRockIfPossible(dx, dy))
                    {
                        MoveCharacter(dx, dy);
                    }
                }
            }


            private bool IsLevelCompleted()
            {

                return plates.All(plate => plate.IsActive);
            }
        }
    }
}

