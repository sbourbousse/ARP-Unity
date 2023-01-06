using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var configText = @"
input_stream: ""in""
output_stream: ""out""
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
";
        var graph = new CalculatorGraph(configText);
        var poller = graph.AddOutputStreamPoller<string>("out").Value();
        graph.StartRun().AssertOk();

        for (var i = 0; i < 10; i++)
        {
            var input = new StringPacket("Hello World!", new Timestamp(i));
            graph.AddPacketToInputStream("in", input).AssertOk();
        }

        graph.CloseInputStream("in").AssertOk();

        var output = new StringPacket();
        while (poller.Next(output))
        {
            Debug.Log(output.Get());
        }

        graph.WaitUntilDone().AssertOk();
        graph.Dispose();

        Debug.Log("Done");
    }
    
    void OnApplicationQuit()
    {
        Protobuf.ResetLogHandler();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
