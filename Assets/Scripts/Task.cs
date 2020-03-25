﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {	
	public enum TaskStatus : byte 
	{     
		Detached, // Task has not been attached to a TaskManager     
		Pending, // Task has not been initialized     
		Working, // Task has been initialized     
		Success, // Task completed successfully     
		Fail, // Task completed unsuccessfully     
		Aborted // Task was aborted 
	}  

	public TaskStatus Status { get; private set; }  

	public bool IsDetached { get { return Status == TaskStatus.Detached;}}
	public bool IsAttached { get { return Status != TaskStatus.Detached;}}
	public bool IsPending { get { return Status == TaskStatus.Pending; } }
	public bool IsWorking { get { return Status == TaskStatus.Working; } }
	public bool IsSuccessful {get{ return Status == TaskStatus.Success;}} 
	public bool IsFailed { get { return Status == TaskStatus.Fail; } }
	public bool IsAborted { get { return Status == TaskStatus.Aborted; } } 
	public bool IsFinished { get { return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted); } }  

	public void Abort() 
	{     
		SetStatus(TaskStatus.Aborted); 
	}

	internal void SetStatus(TaskStatus newStatus) 
	{     
		if (Status == newStatus) return;      
		Status = newStatus;      
		switch (newStatus)     
		{
			case TaskStatus.Working:             
				Initialize();             
				break;
			case TaskStatus.Success:
				OnSuccess();             
				CleanUp();             
				break;          
			case TaskStatus.Aborted:             
				OnAbort();             
				CleanUp();             
				break;
			case TaskStatus.Fail:             
				OnFail();             
				CleanUp();             
				break;          
			case TaskStatus.Detached:
			case TaskStatus.Pending:
				break;
			default:
				throw new System.ArgumentOutOfRangeException(newStatus.ToString(), newStatus, null); 
		} 
	}

	protected virtual void OnAbort() {}
	protected virtual void OnSuccess() {}  
	protected virtual void OnFail() {}

	protected virtual void Initialize() { }  
	internal virtual void Update() { }  
	protected virtual void CleanUp() { }

	public Task NextTask { get; set; }  

	public Task Then(Task task) 
	{     
		Debug.Assert(!task.IsAttached);     
		this.NextTask = task;     
		Debug.Log(NextTask);
		return task; 
	}

}

public class ActionTask : Task {
	private readonly System.Action _action;

	public ActionTask(System.Action action = null)     
	{         
		_action = action ?? (() => { });     
	}      

	protected override void Initialize()     
	{         
		SetStatus(TaskStatus.Success);       
		_action();
	} 
}

public class DelegateTask : Task
{
	public delegate void InitializationHandler();
	public delegate bool UpdateHandler();
	public delegate void SuccessHandler();
	public delegate void FailureHandler();

	private InitializationHandler initialize;
	private UpdateHandler update;
	private SuccessHandler onSuccess;
	private FailureHandler onFailure;

	public DelegateTask(InitializationHandler initialize, UpdateHandler update = null, SuccessHandler onSuccess = null, FailureHandler onFailure = null)
	{
		this.initialize = initialize;
		this.update = update ?? (() => true);

		this.onSuccess = onSuccess;
		this.onFailure = onFailure;
	}

	protected override void Initialize()
	{
		initialize();
	}

	internal override void Update()
	{
		var finished = update();
		
		if (finished) 
			SetStatus(TaskStatus.Success);
	}

	protected override void OnSuccess()
	{
		onSuccess?.Invoke();
	}

	protected override void OnFail()
	{
		onFailure?.Invoke();
	}
}

public class WaitTask : Task 
{    
	private readonly double _duration;     
	private double _elapsedTime;

	public WaitTask(double duration)
	{
		_duration = duration;
	}

	public WaitTask (float duration)
	{
		_duration = duration;
	}

	public WaitTask(int duration)     
	{         
		_duration = duration;
	}     

	protected override void Initialize()     
	{         
		_elapsedTime = 0;     
	}      

	internal override void Update()     
	{         
		_elapsedTime += Time.deltaTime;    

		if (_elapsedTime >= _duration)         
		{             
			SetStatus(TaskStatus.Success);         
		}     
	} 
}