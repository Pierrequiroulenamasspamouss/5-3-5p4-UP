public class SocialPartyFillOrderProfileButtonMediator : global::Kampai.UI.View.KampaiMediator
{
	public class SocialPartyFillOrderProfileButtonMediatorData
	{
		public global::Kampai.Game.UserIdentity identity;

		public global::UnityEngine.GameObject parent;

		public int index;
	}

	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialPartyFillOrderProfileButtonMediator") as global::Kampai.Util.IKampaiLogger;

	private int index;

	private SocialPartyFillOrderProfileButtonMediator.SocialPartyFillOrderProfileButtonMediatorData data;

	[Inject]
	public global::Kampai.UI.View.SocialPartyFillOrderProfileButtonView view { get; set; }

	[Inject]
	public global::Kampai.UI.View.ShowSocialPartyFBConnectSignal showPartyFBConnectSignal { get; set; }

	[Inject]
	public global::Kampai.UI.View.ShowSocialPartyInviteSignal sendSocialPartyInviteSignal { get; set; }

	[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
	public global::Kampai.Game.ISocialService facebookService { get; set; }

	[Inject]
	public global::Kampai.Main.ILocalizationService localService { get; set; }

	[Inject]
	public global::Kampai.UI.View.SocialPartyFillOrderProfileButtonMediatorUpdateSignal socialPartyFillOrderProfileButtonMediatorUpdateSignal { get; set; }

	public override void Initialize(global::Kampai.UI.View.GUIArguments args)
	{
		data = args.Get<SocialPartyFillOrderProfileButtonMediator.SocialPartyFillOrderProfileButtonMediatorData>();
		if (data.parent == null)
		{
			global::UnityEngine.Object.Destroy(view.gameObject);
			return;
		}
		index = data.index;
		global::UnityEngine.RectTransform rectTransform = view.transform as global::UnityEngine.RectTransform;
		rectTransform.SetParent(data.parent.transform);
		rectTransform.localPosition = global::UnityEngine.Vector3.zero;
		rectTransform.localScale = global::UnityEngine.Vector3.one;
		rectTransform.offsetMin = new global::UnityEngine.Vector2(0f, 0f);
		rectTransform.offsetMax = new global::UnityEngine.Vector2(0f, 0f);
		if (index == 0)
		{
			rectTransform.anchorMin = new global::UnityEngine.Vector2(0.02f, 0.52f);
			rectTransform.anchorMax = new global::UnityEngine.Vector2(0.48f, 0.98f);
		}
		else if (index == 1)
		{
			rectTransform.anchorMin = new global::UnityEngine.Vector2(0.52f, 0.52f);
			rectTransform.anchorMax = new global::UnityEngine.Vector2(0.98f, 0.98f);
		}
		else if (index == 2)
		{
			rectTransform.anchorMin = new global::UnityEngine.Vector2(0.02f, 0.02f);
			rectTransform.anchorMax = new global::UnityEngine.Vector2(0.48f, 0.48f);
		}
		else
		{
			rectTransform.anchorMin = new global::UnityEngine.Vector2(0.52f, 0.02f);
			rectTransform.anchorMax = new global::UnityEngine.Vector2(0.98f, 0.48f);
		}
		SetupProfile();
	}

	public override void OnRegister()
	{
		socialPartyFillOrderProfileButtonMediatorUpdateSignal.AddListener(UpdateDetails);
	}

	public override void OnRemove()
	{
		socialPartyFillOrderProfileButtonMediatorUpdateSignal.RemoveListener(UpdateDetails);
		view.addButton.ClickedSignal.RemoveListener(PlusButton);
	}

	public void UpdateDetails(SocialPartyFillOrderProfileButtonMediator.SocialPartyFillOrderProfileButtonMediatorData details)
	{
		if (data != null && details.index == data.index)
		{
			data = details;
			SetupProfile();
		}
	}

	public void SetupProfile()
	{
		global::Kampai.Game.FacebookService facebookService = this.facebookService as global::Kampai.Game.FacebookService;
		if (facebookService.isLoggedIn && data.identity != null)
		{
			view.profileOpenPanel.SetActive(true);
			view.profileClosedPanel.SetActive(false);
			string externalID = data.identity.ExternalID;
			global::strange.extensions.signal.impl.Signal<string> signal = new global::strange.extensions.signal.impl.Signal<string>();
			signal.AddListener(OnFacebookPictureComplete);
			StartCoroutine(facebookService.DownloadUserPicture(externalID, signal));
			global::Kampai.Game.FBUser friend = facebookService.GetFriend(externalID);
			if (friend != null)
			{
				view.profileOpenTextFBName.text = friend.name;
				view.profileOpenTextFBName.gameObject.SetActive(true);
			}
			else
			{
				view.profileOpenTextFBName.gameObject.SetActive(false);
			}
		}
		else
		{
			if (facebookService.isLoggedIn)
			{
				view.AddPlayerIcon.SetActive(true);
				view.loginText.SetActive(false);
			}
			else
			{
				view.AddPlayerIcon.SetActive(false);
				view.loginText.SetActive(true);
			}
			view.profileOpenPanel.SetActive(false);
			view.profileClosedPanel.SetActive(true);
			view.profileOpenTextFBName.gameObject.SetActive(false);
			view.profileCloseTextAvailableName.text = localService.GetString("socialpartyprofileclosetextavailablename");
			view.addButton.ClickedSignal.AddListener(PlusButton);
		}
	}

	public void PlusButton()
	{
		global::Kampai.Game.FacebookService facebookService = this.facebookService as global::Kampai.Game.FacebookService;
		if (facebookService.isLoggedIn)
		{
			sendSocialPartyInviteSignal.Dispatch();
			return;
		}
		this.facebookService.LoginSource = "Social Event";
		showPartyFBConnectSignal.Dispatch(delegate(bool successful)
		{
			if (successful)
			{
				view.AddPlayerIcon.SetActive(true);
				view.loginText.SetActive(false);
				sendSocialPartyInviteSignal.Dispatch();
			}
		});
	}

	private void OnFacebookPictureComplete(string id)
	{
		global::Kampai.Game.FacebookService facebookService = this.facebookService as global::Kampai.Game.FacebookService;
		global::UnityEngine.Texture userPicture = facebookService.GetUserPicture(id);
		if (userPicture != null)
		{
			global::UnityEngine.Sprite sprite = global::UnityEngine.Sprite.Create(userPicture as global::UnityEngine.Texture2D, new global::UnityEngine.Rect(0f, 0f, userPicture.width, userPicture.height), new global::UnityEngine.Vector2(0f, 0f));
			view.profileOpenImageFB.sprite = sprite;
		}
		else
		{
			logger.Warning("OnFacebookPictureComplete null texture for Facebook ID: {0}", id);
		}
	}
}
