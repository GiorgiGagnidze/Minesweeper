

namespace MineSweeper.Game
{
	public class Board
	{
		public const string CEL_PLACE = "Prefabs/Cell";
        public const string CEL = "Cell";
        public const string BOARD = "Board";
		public const int GRID_WIDTH = 10;
		public const int GRID_HEIGHT = 10;
		public const int MAX_BOMB = 10;
		private int [] Bombs;
		public Cell[,] Cells { get; private set; }
		public int NumFlags { get; private set; }
		public GameMode Mode { get; private set; }
		private int CellsOpend; 
		
		private void InitBombs()
		{
			System.Random rnd = new System.Random();			
			Bombs = new int[MAX_BOMB];			
			int sign = -1;
			for (int i = 0; i < MAX_BOMB; i++)			
				Bombs[i] = sign;
			
			for (int i = 0; i < MAX_BOMB; i++)
			{
				while (true)
				{
					int random = rnd.Next(0,GRID_HEIGHT*GRID_WIDTH-1);
					bool toTake = true;
					for (int j = 0; j < MAX_BOMB; j++)
					{
						if (random == Bombs[j])
						{
							toTake =false;
							break;
						}
						if (Bombs[j] == sign)
							break;
					}
					if (toTake)
					{
						Bombs[i] = random;
						break;	
					}
				}
				Cells[Bombs[i]/GRID_WIDTH,Bombs[i]%GRID_HEIGHT] = new Cell(true);
			}
		}
		
		private int Neighbors(int x, int y)
		{
			int count = 0;
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					int x1 = x+i;
					int y1 = y+j;
					if (x1 > -1 && y1 > -1 && x1 < GRID_WIDTH && y1 < GRID_HEIGHT && Cells[x1,y1] != null && Cells[x1,y1].IsBomb)
						count++;					
				}
			}
			return count;
		}
		
		public Board()
		{
			CellsOpend = 0;
			NumFlags = MAX_BOMB;
			Mode = GameMode.Playing;
			Cells = new Cell[GRID_WIDTH,GRID_HEIGHT];
			InitBombs();
			
			for (int i = 0; i < GRID_WIDTH; i++)
			{
				for (int j = 0; j < GRID_HEIGHT; j++)
				{
					if (Cells[i,j] == null)
					{
						Cells[i,j] = new Cell(false);
						Cells[i,j].NeighborCount = Neighbors(i,j);
					}
					
				}
			}
		}

		public void RightClick(int x, int y)
		{
			if (Mode == GameMode.Playing)
			{
				Cell temp = Cells[x,y];
				if (temp.Mode == CellMode.Closed && NumFlags > 0)
				{
					temp.Mode = CellMode.Flaged;
					NumFlags--;
				}else if (temp.Mode == CellMode.Flaged) 
				{
					temp.Mode = CellMode.Closed;
					NumFlags++;
				}
					
			}
		}
		
		public void LeftClick(int x, int y)
		{
			Cell temp = Cells[x,y];
			if (Mode == GameMode.Playing && temp.Mode != CellMode.Open)
			{
				if (CellMode.Flaged == temp.Mode)
				{
					temp.Mode = CellMode.Open;
					NumFlags++;
				}	
				if (temp.IsBomb)
				{
					Mode = GameMode.Lose;
				} else 
				{
					RecursiveOpen(x,y);
				}
				if (GRID_HEIGHT*GRID_WIDTH-CellsOpend == MAX_BOMB)
					Mode = GameMode.Win;
			}
			
		}
		
		public void RecursiveOpen(int x, int y)
		{
			Cell temp = Cells[x,y];
			temp.Mode = CellMode.Open;
			CellsOpend++;
			if (temp.NeighborCount == 0)
			{
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						int x1 = x+i;
						int y1 = y+j;	
						if (x1 > -1 && y1 > -1 && x1 < GRID_WIDTH && y1 < GRID_HEIGHT) 
						{
							Cell curr = Cells[x1,y1];
							if (curr.Mode != CellMode.Open)
								RecursiveOpen(x1,y1);
						}
										
					}
				}
			} 
		}
	}

	public class Cell
	{
		public CellMode Mode { get; set; }
		public bool IsBomb { get; set; }
		public int NeighborCount { get; set; }
		
		public Cell(bool isBomb)
		{
			IsBomb = isBomb;
			this.Mode = CellMode.Closed;
		}
	}
}
