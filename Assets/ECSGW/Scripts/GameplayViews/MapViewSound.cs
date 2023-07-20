using System;
using UnityEngine;

namespace Nashet.GameplayView
{
	public interface IMapViewSound
	{
		bool SlideHappenedHandler(Vector2Int from, Vector2Int to);
	}

	public class MapViewSound : MonoBehaviour, IMapViewSound
	{
		private AudioSource audioData;
		[SerializeField] private AudioClip clip;

		public bool SlideHappenedHandler(Vector2Int from, Vector2Int to)
		{
			AnimateCellSlide(from, to);
			return true;
		}

		private void AnimateCellSlide(Vector2Int from, Vector2Int to)
		{
			audioData.PlayOneShot(clip);
		}

		private void AnimateCellFalling(Vector2Int value)
		{
			throw new NotImplementedException();
		}

		private void AnimateExposion(Vector2Int where, int amount)
		{

		}

		private void ExplosionHappened(Vector2Int where, int amount)
		{
			AnimateExposion(where, amount);
		}

		private void Start()
		{
			audioData = GetComponent<AudioSource>();
		}
	}
}
