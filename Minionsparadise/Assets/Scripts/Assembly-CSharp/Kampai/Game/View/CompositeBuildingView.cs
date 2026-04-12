namespace Kampai.Game.View
{
	public class CompositeBuildingView : global::Kampai.Util.KampaiView
	{
		public global::System.Collections.Generic.List<global::UnityEngine.Transform> SpawnPoints;

		public global::UnityEngine.Transform Placement01VFXPrefab;

		public global::UnityEngine.Transform Placement02VFXPrefab;

		public global::UnityEngine.Transform ShuffleVFXPrefab;

		public global::UnityEngine.Vector3 MidShuffleOffsetTop = new global::UnityEngine.Vector3(-1.732f, 0f, 1f);

		public global::UnityEngine.Vector3 MidShuffleOffsetNotTop = new global::UnityEngine.Vector3(0f, 4f, 0f);

		private global::System.Collections.Generic.IList<global::Kampai.Game.View.CompositeBuildingPieceObject> pieceObjects = new global::System.Collections.Generic.List<global::Kampai.Game.View.CompositeBuildingPieceObject>();

		public void SetupPieces(global::System.Collections.Generic.IList<global::Kampai.Game.CompositeBuildingPiece> compositePieces)
		{
			for (int i = 0; i < compositePieces.Count; i++)
			{
				AddPiece(compositePieces[i]);
			}
			RefreshColliderSize();
			RefreshPieceAppearance();
		}

		public void AddNewlyCreatedPiece(global::Kampai.Game.CompositeBuildingPiece piece, global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal)
		{
			StartCoroutine(WaitThenAddNewPiece(piece, playSFXSignal));
		}

		private global::System.Collections.IEnumerator WaitThenAddNewPiece(global::Kampai.Game.CompositeBuildingPiece piece, global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal)
		{
			yield return new global::UnityEngine.WaitForSeconds(2.2f);
			global::Kampai.Game.View.CompositeBuildingPieceObject newPiece = AddPiece(piece);
			newPiece.PlayFallInAnimation();
			playSFXSignal.Dispatch("Play_totem_fallIn_01");
			RefreshColliderSize();
			yield return new global::UnityEngine.WaitForSeconds(0.4f);
			RefreshPieceAppearance();
			global::UnityEngine.Transform vfxInstanceP = global::UnityEngine.Object.Instantiate(Placement01VFXPrefab);
			vfxInstanceP.SetParent(newPiece.transform, false);
			vfxInstanceP.transform.localPosition = new global::UnityEngine.Vector3(0f, 0.1f, 0f);
			global::UnityEngine.Transform vfxInstanceP2 = global::UnityEngine.Object.Instantiate(Placement02VFXPrefab);
			vfxInstanceP2.SetParent(newPiece.transform, false);
			vfxInstanceP2.transform.localPosition = new global::UnityEngine.Vector3(0f, 0.1f, 0f);
			global::UnityEngine.Transform vfxInstanceP3 = global::UnityEngine.Object.Instantiate(ShuffleVFXPrefab);
			vfxInstanceP3.SetParent(newPiece.transform, false);
			vfxInstanceP3.transform.localPosition = new global::UnityEngine.Vector3(0f, 0.1f, 0f);
		}

		private global::Kampai.Game.View.CompositeBuildingPieceObject AddPiece(global::Kampai.Game.CompositeBuildingPiece piece)
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(piece.Definition.PrefabName);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.Game.View.CompositeBuildingPieceObject component = gameObject.GetComponent<global::Kampai.Game.View.CompositeBuildingPieceObject>();
			global::UnityEngine.Transform transform = component.transform;
			transform.SetParent(base.transform, false);
			transform.localPosition = SpawnPoints[pieceObjects.Count].localPosition;
			transform.localRotation = SpawnPoints[pieceObjects.Count].localRotation;
			component.PieceID = piece.ID;
			pieceObjects.Add(component);
			return component;
		}

		private void RefreshColliderSize()
		{
			float num = 0f;
			for (int i = 0; i < pieceObjects.Count; i++)
			{
				num = global::UnityEngine.Mathf.Max(num, pieceObjects[i].GetMaxBounds().y);
			}
			num -= base.transform.position.y;
			global::UnityEngine.BoxCollider component = GetComponent<global::UnityEngine.BoxCollider>();
			if (component != null)
			{
				component.size = new global::UnityEngine.Vector3(component.size.x, num, component.size.z);
				component.center = new global::UnityEngine.Vector3(component.center.x, num / 2f, component.center.z);
			}
		}

		private void RefreshPieceAppearance()
		{
			int count = pieceObjects.Count;
			for (int i = 0; i < count; i++)
			{
				bool isOnTop = i == count - 1;
				bool allPiecesCollected = count == SpawnPoints.Count;
				pieceObjects[i].RefreshAppearance(isOnTop, allPiecesCollected);
			}
		}

		public int GetNumPieces()
		{
			return pieceObjects.Count;
		}

		public void PlayShuffleSequence(global::System.Collections.Generic.IList<int> newPieceOrder, global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.View.CompositeBuildingPieceObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.CompositeBuildingPieceObject>();
			for (int i = 0; i < newPieceOrder.Count; i++)
			{
				list.Add(getPieceObjectByID(newPieceOrder[i]));
			}
			pieceObjects = list;
			StartCoroutine(ShufflePiecesStaggered(playSFXSignal));
		}

		private global::Kampai.Game.View.CompositeBuildingPieceObject getPieceObjectByID(int pieceID)
		{
			for (int i = 0; i < pieceObjects.Count; i++)
			{
				if (pieceObjects[i].PieceID == pieceID)
				{
					return pieceObjects[i];
				}
			}
			return null;
		}

		private global::System.Collections.IEnumerator ShufflePiecesStaggered(global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal)
		{
			global::UnityEngine.Vector3 squashMove = new global::UnityEngine.Vector3(0f, 0f, 0f);
			float squashAmtUp = 0.38f;
			float squashAmtDown = 0.35f;
			global::Kampai.Game.View.CompositeBuildingPieceObject topPiece = pieceObjects[0];
			topPiece.PlayFallInShuffleTopAnimation();
			TweenTopPieceTo(topPiece, SpawnPoints[0].position, MidShuffleOffsetTop);
			PlayShuffleAudio(playSFXSignal);
			yield return new global::UnityEngine.WaitForSeconds(0.5f);
			for (int i = pieceObjects.Count - 1; i > 0; i--)
			{
				global::Kampai.Game.View.CompositeBuildingPieceObject piece = pieceObjects[i];
				float squashAmtCurrent = (float)(i - 1) * squashAmtUp;
				squashMove = new global::UnityEngine.Vector3(0f, 0f - squashAmtCurrent, 0f);
				piece.PlayJumpAnimation();
				TweenNotTopPieceToUp(piece, MidShuffleOffsetNotTop, squashMove);
			}
			global::UnityEngine.Transform vfxInstance2 = global::UnityEngine.Object.Instantiate(ShuffleVFXPrefab);
			vfxInstance2.SetParent(topPiece.transform, false);
			vfxInstance2.transform.localPosition = new global::UnityEngine.Vector3(0f, 0.1f, 0f);
			topPiece.RefreshAppearance(false, true);
			yield return new global::UnityEngine.WaitForSeconds(0.2f);
			for (int j = 1; j < pieceObjects.Count; j++)
			{
				global::Kampai.Game.View.CompositeBuildingPieceObject piece2 = pieceObjects[j];
				float squashAmtCurrent = ((j != 1) ? squashAmtDown : 0f);
				squashMove = new global::UnityEngine.Vector3(0f, 0f - squashAmtCurrent, 0f);
				yield return new global::UnityEngine.WaitForSeconds(0.1f);
				piece2.PlayFallInShuffleNotTopAnimation();
				TweenNotTopPieceToDown(piece2, SpawnPoints[j].position, squashMove);
			}
			yield return new global::UnityEngine.WaitForSeconds(0.0125f);
			RefreshPieceAppearance();
			yield return new global::UnityEngine.WaitForSeconds(0.25f);
			global::UnityEngine.Transform vfxInstance3 = global::UnityEngine.Object.Instantiate(ShuffleVFXPrefab);
			vfxInstance3.SetParent(pieceObjects[pieceObjects.Count - 1].transform, false);
			vfxInstance3.transform.localPosition = new global::UnityEngine.Vector3(0f, 0.1f, 0f);
		}

		private void PlayShuffleAudio(global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal)
		{
			string type = string.Empty;
			switch (pieceObjects.Count)
			{
			case 2:
				type = "Play_totem_shuffleOfTwo_01";
				break;
			case 3:
				type = "Play_totem_shuffleOfThree_01";
				break;
			case 4:
				type = "Play_totem_shuffleOfFour_01";
				break;
			case 5:
				type = "Play_totem_shuffleOfFive_01";
				break;
			}
			playSFXSignal.Dispatch(type);
		}

		private void TweenTopPieceTo(global::Kampai.Game.View.CompositeBuildingPieceObject piece, global::UnityEngine.Vector3 targetPosition, global::UnityEngine.Vector3 midTweenOffset)
		{
			global::UnityEngine.Transform pieceTransform = piece.transform;
			global::UnityEngine.Vector3 pieceOrigin = pieceTransform.position;
			global::UnityEngine.Vector3 AnticAmt = new global::UnityEngine.Vector3(0.3464f, 0f, -0.2f);
			Go.to(pieceTransform, 0.075f, new GoTweenConfig().setEaseType(GoEaseType.SineIn).position(pieceOrigin + AnticAmt).onComplete(delegate(AbstractGoTween thisTweenA)
			{
				thisTweenA.destroy();
				Go.to(pieceTransform, 0.112500004f, new GoTweenConfig().setEaseType(GoEaseType.SineInOut).position(pieceOrigin + midTweenOffset).onComplete(delegate(AbstractGoTween thisTween)
				{
					thisTween.destroy();
					Go.to(pieceTransform, 7.5E-06f, new GoTweenConfig().setEaseType(GoEaseType.Linear).position(pieceOrigin + midTweenOffset).onComplete(delegate(AbstractGoTween abstractGoTween)
					{
						abstractGoTween.destroy();
						Go.to(pieceTransform, 0.2625f, new GoTweenConfig().setEaseType(GoEaseType.SineIn).position(targetPosition + midTweenOffset).onComplete(delegate(AbstractGoTween abstractGoTween2)
						{
							abstractGoTween2.destroy();
							Go.to(pieceTransform, 0.16874999f, new GoTweenConfig().setEaseType(GoEaseType.Linear).position(targetPosition + midTweenOffset).onComplete(delegate(AbstractGoTween abstractGoTween3)
							{
								abstractGoTween3.destroy();
								Go.to(pieceTransform, 3f / 32f, new GoTweenConfig().setEaseType(GoEaseType.SineInOut).position(targetPosition + AnticAmt).onComplete(delegate(AbstractGoTween thisTweenF)
								{
									thisTweenF.destroy();
									Go.to(pieceTransform, 0.112500004f, new GoTweenConfig().setEaseType(GoEaseType.SineOut).position(targetPosition));
								}));
							}));
						}));
					}));
				}));
			}));
		}

		private void TweenNotTopPieceToUp(global::Kampai.Game.View.CompositeBuildingPieceObject piece, global::UnityEngine.Vector3 midTweenOffset, global::UnityEngine.Vector3 squashOffset)
		{
			global::UnityEngine.Transform pieceTransform = piece.transform;
			global::UnityEngine.Vector3 pieceOrigin = pieceTransform.position;
			Go.to(pieceTransform, 0.09f, new GoTweenConfig().setEaseType(GoEaseType.SineIn).position(pieceTransform.position + squashOffset).onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
				Go.to(pieceTransform, 0.21000001f, new GoTweenConfig().setEaseType(GoEaseType.SineOut).position(pieceOrigin + midTweenOffset));
			}));
		}

		private void TweenNotTopPieceToDown(global::Kampai.Game.View.CompositeBuildingPieceObject piece, global::UnityEngine.Vector3 targetPosition, global::UnityEngine.Vector3 squashOffset)
		{
			global::UnityEngine.Vector3 bounceAmt = new global::UnityEngine.Vector3(0f, 0.2f, 0f);
			global::UnityEngine.Transform pieceTransform = piece.transform;
			Go.to(pieceTransform, 0.21000001f, new GoTweenConfig().setEaseType(GoEaseType.SineIn).position(targetPosition + squashOffset).onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
				Go.to(pieceTransform, 0.060000002f, new GoTweenConfig().setEaseType(GoEaseType.SineOut).position(targetPosition + bounceAmt).onComplete(delegate(AbstractGoTween abstractGoTween)
				{
					abstractGoTween.destroy();
					Go.to(pieceTransform, 0.030000001f, new GoTweenConfig().setEaseType(GoEaseType.SineIn).position(targetPosition));
				}));
			}));
		}
	}
}
