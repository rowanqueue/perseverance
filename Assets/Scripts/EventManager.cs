using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager {
	
	private Dictionary<Type, Eevent.Handler> _registeredHandlers = new Dictionary<Type, Eevent.Handler>();

	public void Register<T>(Eevent.Handler handler) where T : Eevent 
	{
		var type = typeof(T);
		if (_registeredHandlers.ContainsKey(type)) 
		{
			if (!IsEventHandlerRegistered(type, handler))
				_registeredHandlers[type] += handler;         
		} 
		else 
		{
			_registeredHandlers.Add(type, handler);         
		}     
	} 

	public void Unregister<T>(Eevent.Handler handler) where T : Eevent 
	{         
		var type = typeof(T);
		if (!_registeredHandlers.TryGetValue(type, out var handlers)) return;
		
		handlers -= handler;  
		
		if (handlers == null) 
		{                 
			_registeredHandlers.Remove(type);             
		} 
		else
		{
			_registeredHandlers[type] = handlers;             
		}
	}      
		
	public void Fire(Eevent e) 
	{       
		var type = e.GetType();

		if (_registeredHandlers.TryGetValue(type, out var handlers)) 
		{             
			handlers(e);
		}     
	} 

	public bool IsEventHandlerRegistered (Type typeIn, Delegate prospectiveHandler)
	{
		return _registeredHandlers[typeIn].GetInvocationList().Any(existingHandler => existingHandler == prospectiveHandler);
	}
}

public abstract class Eevent 
{
	public readonly float creationTime;

	public Eevent ()
	{
		creationTime = Time.time;
	}

	public delegate void Handler (Eevent e);
}
public class PlacedOnCache : Eevent {}
