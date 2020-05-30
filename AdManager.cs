using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardVideoAd;

    void Start() {
        MobileAds.Initialize(initStatus => {});

        requestBanner();
        requestInterstitial();
        requestRewardVideoAd();
    }

    void requestBanner() {
        string bannerId = "ca-app-pub-5735782229237225/8173123708";

        bannerAd = new BannerView(bannerId, AdSize.SmartBanner, AdPosition.Top);

        AdRequest adRequest = new AdRequest.Builder().Build();

        bannerAd.LoadAd(adRequest);
    }

    void requestInterstitial() {
        string interstitialId = "ca-app-pub-5735782229237225/8193019590";

        interstitialAd = new InterstitialAd(interstitialId);

        AdRequest adRequest = new AdRequest.Builder().Build();

        interstitialAd.LoadAd(adRequest);
    }

    void requestRewardVideoAd() {
        string videoAdId = "ca-app-pub-5735782229237225/9474311675";

        rewardVideoAd = RewardBasedVideoAd.Instance;

        AdRequest adRequest = new AdRequest.Builder().Build();

        rewardVideoAd.LoadAd(adRequest, videoAdId);
    }

    public void displayBanner() {
        bannerAd.Show();
    }

    public void displayInterstitialAd() {
        if (interstitialAd.IsLoaded()) {
            interstitialAd.Show();
        }
    }

    public void displayRewardVideoAd() {
        if (rewardVideoAd.IsLoaded()) {
            rewardVideoAd.Show();
        }
    }

    // HANDLE EVENTS

    public void HandleOnAdLoaded(object sender, EventArgs args) {
        // Ad is loaded, show it
        displayBanner();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        // Ad failed to load, load it again
        requestBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args) {
        // User clicked on the Ad, what do you want to do?
    }

    public void HandleOnAdClosed(object sender, EventArgs args) {
        // When ad is closed, more for intertitial ad
        // Here th user watched the ad, see how to reward the player
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args) {
        // He didnt explained in the video, read the documentation
    }

    void handleBannerAdEvents(bool subscribe) {
        if (subscribe) {
            // Called when an ad request has successfully loaded.
            this.bannerAd.OnAdLoaded += this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerAd.OnAdOpening += this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerAd.OnAdClosed += this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerAd.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
        } else {
            // Called when an ad request has successfully loaded.
            this.bannerAd.OnAdLoaded -= this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerAd.OnAdFailedToLoad -= this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerAd.OnAdOpening -= this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerAd.OnAdClosed -= this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerAd.OnAdLeavingApplication -= this.HandleOnAdLeavingApplication;
        }
    }

    // HANDLE REWARDED VIDEO EVENTS

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        // Video loaded, show it
        displayRewardVideoAd();
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // Video failed to load, load it again
        requestRewardVideoAd();
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        // Do something when the video is opened
        // I think i will not use this
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        // Do something when the video start
        // I think i will not use this
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        // whe the video is closed
        // i think here the user didnt watched the full video
        // just give the normal offline earnings reward
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        // Give the player the reward for watching the video
        // Load the game manager
        // get the offline earnings
        // add the double of offline earnings to total souls
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        // Check the documentation
        // I think i will not use this
    }

    void OnEnable() {
        handleBannerAdEvents(true);
    }

    void OnDisable() {
        handleBannerAdEvents(false);
    }
}
