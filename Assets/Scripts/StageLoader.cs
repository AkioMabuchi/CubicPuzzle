using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Stages
{
    public Stage[] stages;
}

[Serializable]
public class Stage
{
    public string title;
    public string name;
    public string floors;
    public string levels;
    public string objects;
}

public interface IStageLoader
{
    public IObservable<Stage[]> ReceiveStageResult
    {
        get;
    }
    public IObservable<bool> SendStageResult
    {
        get;
    }
    public void ReceiveStages();
    public void SendStage(Stage stage);
}

public class StageLoader : MonoBehaviour, IStageLoader
{
    [SerializeField] private string accessKey;

    private readonly Subject<Stage[]> _receiveStageResult = new Subject<Stage[]>();
    private readonly Subject<bool> _sendStageResult = new Subject<bool>();

    public IObservable<Stage[]> ReceiveStageResult => _receiveStageResult;
    public IObservable<bool> SendStageResult => _sendStageResult;

    public void ReceiveStages()
    {
        StartCoroutine(CoroutineReceiveStages());
    }

    public void SendStage(Stage stage)
    {
        StartCoroutine(CoroutineSendStage(stage));
    }
    
    IEnumerator CoroutineReceiveStages()
    {
        UnityWebRequest request
            = UnityWebRequest.Get("https://records.akiomabuchi.com/" + accessKey + "/receive");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            _receiveStageResult.OnNext(null);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Stages stages = JsonUtility.FromJson<Stages>(request.downloadHandler.text);
                _receiveStageResult.OnNext(stages.stages);
            }
            else
            {
                _receiveStageResult.OnNext(null);
            }
        }
    }

    IEnumerator CoroutineSendStage(Stage stage)
    {
        WWWForm form = new WWWForm();
        form.AddField("title", stage.title);
        form.AddField("name", stage.name);
        form.AddField("floors", stage.floors);
        form.AddField("levels", stage.levels);
        form.AddField("objects", stage.objects);

        UnityWebRequest request =
            UnityWebRequest.Post("https://records.akiomabuchi.com/" + accessKey + "/send", form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            _sendStageResult.OnNext(false);
        }
        else
        {
            if (request.responseCode == 204)
            {
                _sendStageResult.OnNext(true);
            }
            else
            {
                _sendStageResult.OnNext(false);
            }
        }
    }
}
