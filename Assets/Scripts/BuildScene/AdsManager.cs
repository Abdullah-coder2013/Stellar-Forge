using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private string newLife = "ca-app-pub-7134863660692852/2428370755";
    private string prizeWheel = "ca-app-pub-7134863660692852/1115289086";
    
    public RewardedAd newLifeRewardedAd;
    public RewardedAd prizeWheelRewardedAd;

    private string adKeywords = "unity-admob-game";

    private void LoadRewardedAd(string adUnitId)
    {
        if (adUnitId == newLife)
        {
            if (newLifeRewardedAd != null){
                newLifeRewardedAd.Destroy();
                newLifeRewardedAd = null;
            }

            var adrequest = new AdRequest();
            adrequest.Keywords.Add(adKeywords);
            RewardedAd.Load(adUnitId, adrequest, ((RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    print("Ad Failed To Load");
                    return;
                }

                newLifeRewardedAd = ad;
                RewardedAdEvents(newLifeRewardedAd);
            }));

        }
        else if (adUnitId == prizeWheel)
        {
            if (prizeWheelRewardedAd != null){
                prizeWheelRewardedAd.Destroy();
                prizeWheelRewardedAd = null;
            }

            var adrequest = new AdRequest();
            adrequest.Keywords.Add(adKeywords);
            RewardedAd.Load(adUnitId, adrequest, ((RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    print("Ad Failed To Load");
                    return;
                }

                prizeWheelRewardedAd = ad;
                RewardedAdEvents(prizeWheelRewardedAd);
            }));
        }
    }

    public bool ShowRewardedAd(string adUnitId)
    {
        if (adUnitId == newLife)
        {
            LoadRewardedAd(adUnitId);
            if (newLifeRewardedAd != null && newLifeRewardedAd.CanShowAd())
            {
                newLifeRewardedAd.Show((GoogleMobileAds.Api.Reward reward) =>
                {
                    print("Reward received for watching ad.");
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (adUnitId == prizeWheel)
        {
            LoadRewardedAd(adUnitId);
            if (prizeWheelRewardedAd != null && prizeWheelRewardedAd.CanShowAd())
            {
                prizeWheelRewardedAd.Show((GoogleMobileAds.Api.Reward reward) =>
                {
                    print("Reward received for watching ad.");
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
