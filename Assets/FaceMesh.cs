using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine;
using UnityEngine.UI;

public class FaceMesh : MonoBehaviour
{

   private WebCamTexture _webCamTexture;
   //private TextAsset _configAsset;
   private RawImage _screen;
   private int _width = 240;
   private int _height = 480;
   private int _fps = 30;

   private CalculatorGraph _graph;
   private Texture2D _inputTexture;
   private Color32[] _pixelData;
   
   IEnumerator Start()
   {
      _webCamTexture = new WebCamTexture();
      _screen = GetComponent<RawImage>();
      Renderer renderer = GetComponent<Renderer>();

      if (WebCamTexture.devices.Length == 0)
      {
         throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
      _webCamTexture.Play();

      yield return new WaitUntil(() => _webCamTexture.width > 16);

      renderer.material.mainTexture = _webCamTexture;
//
//       // _screen.rectTransform.sizeDelta = new Vector2(_width, _height);
//       // _screen.texture = _webCamTexture;

      _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
      _pixelData = new Color32[_width * _height];

      
      var graph = new CalculatorGraph(@"
node {
calculator: ""FlowLimiterCalculator""
   input_stream: ""input_video""
   input_stream: ""FINISHED:output_video""
   input_stream_info: {
      tag_index: ""FINISHED""
      back_edge: true
   }
   output_stream: ""throttled_input_video""
}

# Defines side packets for further use in the graph.
node {
   calculator: ""ConstantSidePacketCalculator""
   output_side_packet: ""PACKET:0:num_faces""
   output_side_packet: ""PACKET:1:with_attention""
   node_options: {
      [type.googleapis.com/mediapipe.ConstantSidePacketCalculatorOptions]: {
         packet { int_value: 1 }
         packet { bool_value: true }
      }
   }
}

# Subgraph that detects faces and corresponding landmarks.
node {
   calculator: ""FaceLandmarkFrontCpu""
   input_stream: ""IMAGE:throttled_input_video""
   input_side_packet: ""NUM_FACES:num_faces""
   input_side_packet: ""WITH_ATTENTION:with_attention""
   output_stream: ""LANDMARKS:multi_face_landmarks""
   output_stream: ""ROIS_FROM_LANDMARKS:face_rects_from_landmarks""
   output_stream: ""DETECTIONS:face_detections""
   output_stream: ""ROIS_FROM_DETECTIONS:face_rects_from_detections""
}

# Subgraph that renders face-landmark annotation onto the input image.
node {
   calculator: ""FaceRendererCpu""
   input_stream: ""IMAGE:throttled_input_video""
   input_stream: ""LANDMARKS:multi_face_landmarks""
   input_stream: ""NORM_RECTS:face_rects_from_landmarks""
   input_stream: ""DETECTIONS:face_detections""
   output_stream: ""IMAGE:output_video""
}  
"
      );
      graph.StartRun().AssertOk();
      
//
//       while (true)
//       {
//          _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_pixelData));
//          var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4, _inputTexture.GetRawTextureData<byte>());
//          _graph.AddPacketToInputStream("input_video", new ImageFramePacket(imageFrame)).AssertOk();
//
//          yield return new WaitForEndOfFrame();
//       }


   }
   
   private void OnDestroy()
   {
      // if (_webCamTexture != null)
      // {
      //    _webCamTexture.Stop();
      // }
      //
      // if (_graph != null)
      // {
      //    try
      //    {
      //       _graph.CloseInputStream("input_video").AssertOk();
      //       _graph.WaitUntilDone().AssertOk();
      //    }
      //    finally
      //    {
      //
      //       _graph.Dispose();
      //    }
      // }
   }
}