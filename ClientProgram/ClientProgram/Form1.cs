using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ClientProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool isKeyCommandActivated;
        private bool isConnectedToServer;
        private List<Point> bricks = new List<Point>();
        private List<Point> stones = new List<Point>();
        private List<Point> water = new List<Point>();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (isConnectedToServer == false)
            {
                Thread aThread = new Thread(new ThreadStart(ConnectAsClient));
                aThread.Start();

                Thread bThread = new Thread(new ThreadStart(Listen));
                bThread.Start();

                isConnectedToServer = true;
                btnConnect.BackColor = Color.Green;
                btnConnect.Text = "Connected !";
            }
            else
            {
                MessageBox.Show("You are already connected to the server");
            }


        }


        private void ConnectAsClient(){

            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"),6000);
            UpdateUI("Connected !");

            NetworkStream stream = client.GetStream();
            String s = "JOIN#";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message,0,message.Length);
            UpdateUI("Game inixialization Message Sent !");
            stream.Close();
            client.Close();
        }


        private void Command(String Command)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            UpdateUI("Connected !");

            NetworkStream stream = client.GetStream();
            byte[] message = Encoding.ASCII.GetBytes(Command);
            stream.Write(message, 0, message.Length);
            UpdateUI(Command);
            stream.Close();
            client.Close();
        }


        private void Left()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            UpdateUI("Connected !");

            NetworkStream stream = client.GetStream();
            String s = "LEFT#";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
            UpdateUI("LEFT#");
            stream.Close();
            client.Close();
        }


        private void Up()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            UpdateUI("Connected !");

            NetworkStream stream = client.GetStream();
            String s = "UP#";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
            UpdateUI("UP#");
            stream.Close();
            client.Close();
        }


        private void Down()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            UpdateUI("Connected !");

            NetworkStream stream = client.GetStream();
            String s = "DOWN#";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
            UpdateUI("DOWN#");
            stream.Close();
            client.Close();
        }


        private void Shoot()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            UpdateUI("Connected !");

            NetworkStream stream = client.GetStream();
            String s = "SHOOT#";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
            UpdateUI("SHOOT#");
            stream.Close();
            client.Close();
        }


        private void UpdateUI(String s)
        {
            Func<int> del = delegate()
            {
                textBox1.AppendText(s+ System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }


        private void UpdateKeyEventUI(String s)
        {
            Func<int> del = delegate()
            {
                txtKeyDisplay.Text = s;
                return 0;
            };
            Invoke(del);
        }


        private void Listen()
        {
            const int PORT_NO = 7000;
            const string SERVER_IP = "127.0.0.1";


            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);

            listener.Start();


            while (true)
            {
                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                displayGame(dataReceived);
                UpdateUI("Received : " + dataReceived);

            }
        }
        private string[][] initGame()
        {
            string[][] game = new string[20][];
            for (int x = 0; x < 20; x++)
            {
                game[x] = new string[20];
            }
            for (int x = 0; x < 20; x++)
            {
                for (int j = 0; j < 20; j++)
                {
                    game[x][j] = "N";
                }
                
            }
            return game;
            
        }
        private void gameMap(string [][]game)
        {
            string gamestring = "";
            for (int x = 0; x < game.Length; x++)
            {
                for (int j = 0; j < game[x].Length; j++)
                {
                    gamestring += game[x][j]+" ";
                }
                gamestring += Environment.NewLine;
            }
            updateTexbox2(gamestring);
        }
        private void updateTexbox2(string s)
        {
            Func<int> del = delegate()
            {
               // textBox2.Text = s;
                textBox2.AppendText(s + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }
        private void displayGame(string s)
        {
            string[][] game = initGame();
            if(s[0]== 'I'){
                updateTexbox2("get init");
                string[] tokens = s.Split(new char[] { ':' });
                //get bricks cordinates
                string[] tokens2 = tokens[2].Split(new char[] { ';' });
                for (int j = 0; j < tokens2.Length; j++)
                {
                    string[] cordinates = tokens2[j].Split(new char[] { ',' });
                    updateTexbox2(cordinates[0] + "   " + cordinates[1]);
                    game[int.Parse(cordinates[0])][int.Parse(cordinates[1])] = "B";
                }
                //get stones cordinates
                string[] tokens3 = tokens[3].Split(new char[] { ';' });
                for (int j = 0; j < tokens3.Length; j++)
                {
                    string[] cordinates = tokens3[j].Split(new char[] { ',' });
                    updateTexbox2(cordinates[0] + "   " + cordinates[1]);
                    game[int.Parse(cordinates[0])][int.Parse(cordinates[1])] = "S";
                }
                //get water cordinates
                string[] tokens4 = tokens[4].Split(new char[] { ';' });
                for (int j = 0; j < tokens2.Length; j++)
                {
                    string[] cordinates = tokens4[j].Split(new char[] { ',' });
                    updateTexbox2(cordinates[0] + "   " + cordinates[1]);
                    game[int.Parse(cordinates[0])][int.Parse(cordinates[1])] = "W";
                }

                gameMap(game);
                /*for (int i = 2; i < tokens.Length; i++)
                {
                    //get bricks cordinates
                    string[] tokens2 = tokens[i].Split(new char[] { ';' });
                   /* for (int j = 0; j < tokens2.Length; j++)
                    {string[] cordinates = tokens2[j].Split(new char[] { ',' });
                        updateTexbox2(cordinates[0] + "   " + cordinates[1]);
                    }
                } */       
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Command("RIGHT#");
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Command("LEFT#");
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            Command("UP#");
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            Command("DOWN#");
        }

        private void btnShoot_Click(object sender, EventArgs e)
        {
            Command("SHOOT#");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           


        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            


        }

        private void btnActiveKeys_Click(object sender, EventArgs e)
        {
            txtKeyDisplay.Text = "";

            if(isKeyCommandActivated==false){
                isKeyCommandActivated = true;
                txtKeyDisplay.Enabled = true;
                lblMessage.Enabled = true;
                btnActiveKeys.BackColor = Color.Green;
                btnActiveKeys.Text = "Deactivate Key Events";
            }
            else
            {
                isKeyCommandActivated = false;
                txtKeyDisplay.Enabled = false;
                lblMessage.Enabled = false;
                btnActiveKeys.BackColor = Color.Red;
                btnActiveKeys.Text = "Activate Key Events";
            }
        }

        private void txtKeyDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Command("SHOOT#");
                UpdateKeyEventUI("SHOOT");
            }
            else if (e.KeyCode == Keys.Up)
            {
                Command("UP#");
                UpdateKeyEventUI("UP");
            }
            else if (e.KeyCode == Keys.Right)
            {
                Command("RIGHT#");
                UpdateKeyEventUI("RIGHT");
            }
            else if (e.KeyCode == Keys.Down)
            {
                Command("DOWN#");
                UpdateKeyEventUI("DOWN");
            }
            else if (e.KeyCode == Keys.Left)
            {
                Command("LEFT#");
                UpdateKeyEventUI("LEFT");
            }
        }

        



    }
}
