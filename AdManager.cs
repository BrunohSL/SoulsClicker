using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardVideoAd;

    void Start() {
        MobileAds.Initialize(initStatus => { });

        this.rewardVideoAd = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when the user should be rewarded for watching a video.
        rewardVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardVideoAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        requestInterstitial();
        requestRewardVideoAd();
    }

    void requestInterstitial() {
        string interstitialId = "ca-app-pub-5735782229237225/8193019590";

        interstitialAd = new InterstitialAd(interstitialId);

        AdRequest adRequest = new AdRequest.Builder().Build();

        interstitialAd.LoadAd(adRequest);
    }

    void requestRewardVideoAd() {
        string videoAdId = "ca-app-pub-5735782229237225/9474311675  ";

        // rewardVideoAd = RewardBasedVideoAd.Instance;

        AdRequest adRequest = new AdRequest.Builder().Build();

        this.rewardVideoAd.LoadAd(adRequest, videoAdId);
    }

    public void displayInterstitialAd() {
        if (interstitialAd.IsLoaded()) {
            interstitialAd.Show();
        }
    }

    public void displayRewardVideoAd() {
        if (this.rewardVideoAd.IsLoaded()) {
            this.rewardVideoAd.Show();
        }
    }

    // HANDLE REWARDED VIDEO EVENTS

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
        // Video loaded, show it
        displayRewardVideoAd();
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        // Video failed to load, load it again
        requestRewardVideoAd();
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args) {
        // Do something when the video is opened
        // I think i will not use this
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args) {
        // Do something when the video start
        // I think i will not use this
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args) {
        // whe the video is closed
        // i think here the user didnt watched the full video
        // just give the normal offline earnings reward
    }

    public void HandleRewardBasedVideoRewarded(object sender, EventArgs args) {
        GameObject gameManagerObj = GameObject.FindGameObjectWithTag("gameManager");
        GameController gameController = gameManagerObj.GetComponent<GameController>();

        gameController.offlineEarningButton(true);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args) {
        // Check the documentation
        // I think i will not use this
    }
}
