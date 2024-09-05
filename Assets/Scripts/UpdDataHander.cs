using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UpdDataHander : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int portToGet;
    public bool ShouldGet = true;
    public bool ConsoleLogData = false;
    public string data = "0";
    // Start is called before the first frame update
    void Start()
    {
        receiveThread = new Thread( new ThreadStart(GetData) );
        receiveThread.IsBackground = true;
        receiveThread.Start();
        
    }

    private void GetData(){
        client = new UdpClient(portToGet);
        while(ShouldGet){
            try{
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any,0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if(ConsoleLogData){ Debug.Log(data);}
            }
            catch(Exception err){
                Debug.Log(err.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
