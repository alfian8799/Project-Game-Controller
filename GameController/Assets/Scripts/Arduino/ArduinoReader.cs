using UnityEngine;
using System.IO.Ports;

public class ArduinoReader : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM5";   // ganti sesuai punya kamu
    public int baudRate = 115200;

    public Vector3 acceleration;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 20;

        try
        {
            serialPort.Open();
            Debug.Log("Serial Connected");
        }
        catch
        {
            Debug.LogError("Gagal buka COM port!");
        }
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();   // baca "x,y,z"
                string[] values = data.Split(',');

                if (values.Length == 3)
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);

                    acceleration = new Vector3(x, y, z);
                }
            }
            catch { }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
            serialPort.Close();
    }
}
