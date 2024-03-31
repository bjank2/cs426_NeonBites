using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using HuggingFace.API;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public class NPC : MonoBehaviour
{
    private bool isRecording = false;
    private AudioSource audioSource;
    private AudioClip recordedClip;
    public Button startButton;
    public TextMeshProUGUI text;

    public string[] targetWords;

    AudioClip clip;
    byte[] bytes;
    bool recording;


    public GameObject x;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("audiolog Collision Player");

            if (!recording)
            {
                startRecord();
            }
            // Optionally, you can handle the case when recording is already in progress.
            // You may choose to stop the recording or handle it differently based on your requirements.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && recording)
        {
             StopRecording();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //set start button bg to green
        //startButton.onClick.AddListener(startRecord);
        Debug.Log("audiolog starting recording");
        recording = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (recording && Microphone.GetPosition(null) >= clip.samples)
        {
            //set start button bg to red
            StopRecording();
        }

    }

    private void StopRecording()
    {
        Debug.Log("audiolog stop recording");
        recording = false;
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        SendRecording();
    }

    void startRecord()
    {
        //text.color = Color.white;
        // text.text = "Recording...";
        clip = Microphone.Start(null, false, 15, 44100);
        recording = true;

    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

    private void SendRecording()
    {
        Debug.Log("audiolog sending recording");
        //text.color = Color.yellow;
        //text.text = "Sending...";

        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response =>
        {
            //text.color = Color.white;
            //text.text = response;
            //startButton.interactable = true;
            Debug.Log("audioolog Succes. Received responce" + response);

            response = response.ToLower();

            recordthing(response);
            //LoadObject(index);
        }, error =>
        {
            //text.color = Color.red;
            //text.text = error;
            //startButton.interactable = true;
        });

    }

    private string recordthing(string sentence)
    {
        // Your sentence

        // Target words


        // Search for the first matching word
        string firstFoundWord = SearchForFirstWord(sentence, targetWords, StringComparison.OrdinalIgnoreCase);

        if (!string.IsNullOrEmpty(firstFoundWord))
        {
            Debug.Log($" audiolog The first found word is: {firstFoundWord}");
            return firstFoundWord;

        }
        else
        {
            Debug.Log("audiolog None of the target words were found in the sentence.");
            return null;
        }
    }

    static string SearchForFirstWord(string sentence, string[] targetWords, StringComparison comparison)
    {
        string[] wordsInSentence = sentence.Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in wordsInSentence)
        {
            if (targetWords.Any(targetWord => string.Equals(word, targetWord, StringComparison.OrdinalIgnoreCase)))
            {
                return word;
            }
        }

        // If no match is found
        return null;
    }
}
