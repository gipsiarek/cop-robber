using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wpfXbap
{
    class DoTestowania
    {
        private void runRobberAutoRun()
        {

            FileStream fs = new FileStream(@"d:\testAutoGreedyDijkstra2Cop.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            #region greedy
            if (isGreedy)
            {
                for (int i = 0; i != 10; i++)
                {
                    //graf mały 
                    currentTest = i;
                    newDataAutoTest(2, 50, 200, 20);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 50 | 200 | 20");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 30);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 50 | 200 | 30");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 40);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 50 | 200 | 40");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 50);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 50 | 200 | 50");
                    resetCops();
                    sw.Flush();

                    //graf średni
                    newDataAutoTest(2, 200, 500, 50);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 200 | 500 | 50");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 75);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 200 | 500 | 75");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 100);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 200 | 500 | 100");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 150);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 200 | 500 | 150");
                    resetCops();
                    sw.Flush();

                    //graf duzy
                    newDataAutoTest(2, 500, 1000, 100);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 500 | 1000 | 100");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 150);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 500 | 1000 | 150");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 200);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 500 | 1000 | 200");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 300);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dijkstra_1vs_many(2).ToString() + " | 500 | 1000 | 300");
                    resetCops();
                    sw.Flush();
                }
                sw.Flush();
                sw.Close();
                fs.Close();
                fs = new FileStream(@"d:\testAutoGreedyDumb2Cop.txt", FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                for (int i = 0; i != 10; i++)
                {
                    //graf mały 
                    currentTest = i;
                    newDataAutoTest(2, 50, 200, 20);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 50 | 200 | 20");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 30);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 50 | 200 | 30");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 40);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 50 | 200 | 40");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 50);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 50 | 200 | 50");
                    resetCops();
                    sw.Flush();

                    //graf średni
                    newDataAutoTest(2, 200, 500, 50);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 200 | 500 | 50");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 75);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 200 | 500 | 75");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 100);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 200 | 500 | 100");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 150);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 200 | 500 | 150");
                    resetCops();
                    sw.Flush();

                    //graf duzy
                    newDataAutoTest(2, 500, 1000, 100);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 500 | 1000 | 100");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 150);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 500 | 1000 | 100");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 200);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 500 | 1000 | 100");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 300);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(greedy_dumb_1vsMany(2).ToString() + " | 500 | 1000 | 100");
                    resetCops();
                    sw.Flush();
                }
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            #endregion

            sw.Flush();
            sw.Close();
            fs.Close();
            fs = new FileStream(@"d:\testAutoRandBeacon2Cop.txt", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            #region RandomBeacon
            if (isRandomB)
            {
                for (int i = 0; i != 10; i++)
                {
                    //graf mały 
                    currentTest = i;
                    newDataAutoTest(2, 50, 200, 20);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(10, 10, 2).ToString() + "| 10 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(10, 20, 2).ToString() + "| 10 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 5, 2).ToString() + "| 40 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 10, 2).ToString() + "| 40 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 20, 2).ToString() + "| 40 | 20 ");
                    resetCops();
                    sw.Flush();


                    newDataAutoTest(2, 50, 200, 30);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(10, 10, 2).ToString() + "| 10 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(10, 20, 2).ToString() + "| 10 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 5, 2).ToString() + "| 40 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 10, 2).ToString() + "| 40 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 20, 2).ToString() + "| 40 | 20 ");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 40);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(10, 10, 2).ToString() + "| 10 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(10, 20, 2).ToString() + "| 10 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 5, 2).ToString() + "| 40 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 10, 2).ToString() + "| 40 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(40, 20, 2).ToString() + "| 40 | 20 ");
                    resetCops();
                    sw.Flush();

                    //graf średni
                    newDataAutoTest(2, 200, 500, 50);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(randomBeacon(50, 5, 2).ToString() + "| 50 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 10, 2).ToString() + "| 50 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 20, 2).ToString() + "| 50 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 5, 2).ToString() + "| 150 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 10, 2).ToString() + "| 150 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 20, 2).ToString() + "| 150 | 20 ");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 75);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(randomBeacon(50, 5, 2).ToString() + "| 50 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 10, 2).ToString() + "| 50 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 20, 2).ToString() + "| 50 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 5, 2).ToString() + "| 150 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 10, 2).ToString() + "| 150 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 20, 2).ToString() + "| 150 | 20 ");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 100);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 5, 2).ToString() + "| 50 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 10, 2).ToString() + "| 50 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 20, 2).ToString() + "| 50 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 5, 2).ToString() + "| 150 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 10, 2).ToString() + "| 150 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 20, 2).ToString() + "| 150 | 20 ");
                    resetCops();
                    sw.Flush();

                    //graf duzy
                    newDataAutoTest(2, 500, 1000, 100);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 5, 2).ToString() + "| 50 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 10, 2).ToString() + "| 50 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 20, 2).ToString() + "| 50 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 5, 2).ToString() + "| 150 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 10, 2).ToString() + "| 150 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 20, 2).ToString() + "| 150 | 20 ");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 150);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 5, 2).ToString() + "| 50 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 10, 2).ToString() + "| 50 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 20, 2).ToString() + "| 50 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 5, 2).ToString() + "| 150 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 10, 2).ToString() + "| 150 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 20, 2).ToString() + "| 150 | 20 ");
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 200);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(randomBeacon(10, 5, 2).ToString() + "| 10 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 5, 2).ToString() + "| 50 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 10, 2).ToString() + "| 50 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(50, 20, 2).ToString() + "| 50 | 20 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 5, 2).ToString() + "| 150 | 5 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 10, 2).ToString() + "| 150 | 10 ");
                    resetCops();
                    sw.WriteLine(randomBeacon(150, 20, 2).ToString() + "| 150 | 20 ");
                    resetCops();
                    sw.Flush();
                }
            }
            #endregion

            sw.Flush();
            sw.Close();
            fs.Close();
            fs = new FileStream(@"d:\testAutoMCTS2Cops.txt", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            #region MCTS
            if (isMCTS)
            {
                for (int i = 0; i != 10; i++)
                {
                    //graf mały 
                    currentTest = i;
                    newDataAutoTest(2, 50, 200, 20);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();


                    newDataAutoTest(2, 50, 200, 30);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 40);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();

                    //graf średni
                    newDataAutoTest(2, 200, 500, 50);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 75);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 3, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();


                    newDataAutoTest(2, 200, 500, 100);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();

                    //graf duzy
                    newDataAutoTest(2, 500, 1000, 100);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 150);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 200);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    txbTreeWidth = 2; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(2, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 2; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(2, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 2;
                    sw.WriteLine(MCTS(4, 2, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(4, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    txbTreeWidth = 6; txbTreeDepth = 4;
                    sw.WriteLine(MCTS(6, 4, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    sw.Flush();
                    resetCops();
                    txbTreeWidth = 4; txbTreeDepth = 6;
                    sw.WriteLine(MCTS(4, 6, 2).ToString() + " | " + txbTreeWidth.ToString() + " | " + txbTreeDepth.ToString());
                    resetCops();
                    sw.Flush();
                }
            }
            #endregion


            sw.Flush();
            sw.Close();
            fs.Close();
            fs = new FileStream(@"d:\testAutoAlfaBet.txt", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            #region AlphaBEta
            if (isAlphaBeta)
            {
                for (int i = 0; i != 10; i++)
                {
                    //graf mały 
                    currentTest = i;
                    newDataAutoTest(1, 50, 200, 20);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();


                    newDataAutoTest(1, 50, 200, 30);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(1, 50, 200, 40);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    //graf średni
                    newDataAutoTest(1, 200, 500, 50);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(1, 200, 500, 75);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(1, 200, 500, 100);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    //graf duzy
                    newDataAutoTest(1, 500, 1000, 100);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(1, 500, 1000, 150);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(1, 500, 1000, 200);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(alphaBetavsGreedy(1, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 1).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 1).ToString());
                    resetCops();
                    sw.Flush();

                }
            }
            #endregion

            sw.Flush();
            sw.Close();
            fs.Close();


            fs = new FileStream(@"d:\testAutoAlfaBet2Cops.txt", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            #region AlphaBEta
            if (isAlphaBeta)
            {
                for (int i = 0; i != 10; i++)
                {
                    //graf mały 
                    currentTest = i;
                    newDataAutoTest(2, 50, 200, 20);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();


                    newDataAutoTest(2, 50, 200, 30);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 50, 200, 40);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    //graf średni
                    newDataAutoTest(2, 200, 500, 50);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 75);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 200, 500, 100);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 5;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    //graf duzy
                    newDataAutoTest(2, 500, 1000, 100);        //zadki
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 150);        //sredni
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    maxMoves = board.verticies * 10;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                    newDataAutoTest(2, 500, 1000, 200);        //gesty
                    checkClassOfGraph();
                    maxMoves = board.verticies * 2;
                    sw.WriteLine(alphaBetavsGreedy(1, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(2, 2).ToString());
                    resetCops();
                    sw.WriteLine(alphaBetavsGreedy(3, 2).ToString());
                    resetCops();
                    sw.Flush();

                }
            }
            #endregion

            sw.Flush();
            sw.Close();
            fs.Close();
            //randomBeacon(10, 5, 1);
            //resetCops();
            //alphaBetavsGreedy(3, 1);
            //resetCops();
            //MCTS(3, 4, 1);
            //resetCops();

        }
    }
}
