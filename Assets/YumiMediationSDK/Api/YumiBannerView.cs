﻿using System;
using YumiMediationSDK.Common;
using System.Reflection;

namespace YumiMediationSDK.Api
{
    public class YumiBannerView
    {
        private IYumiBannerClient client;
        private YumiBannerViewOptions bannerOptions;
        /// <summary>
        /// Creates a BannerView and adds it to the view hierarchy.
        /// </summary>
        /// <param name="placementId">Placement identifier.</param>
        /// <param name="channelId">Channel identifier.</param>
        /// <param name="versionId">Version identifier.</param>
        /// <param name="bannerOptions">Banner options.</param>
        public YumiBannerView(string placementId, string channelId, string versionId, YumiBannerViewOptions bannerOptions)
        {
            Type yumiAdsClientFactory = Type.GetType(
                "YumiMediationSDK.YumiAdsClientFactory,Assembly-CSharp");
            MethodInfo method = yumiAdsClientFactory.GetMethod(
                "BuildBannerClient",
                BindingFlags.Static | BindingFlags.Public);
            this.client = (IYumiBannerClient)method.Invoke(null, null);
            this.bannerOptions = bannerOptions;
            client.CreateBannerView(placementId,channelId,versionId, bannerOptions);

            ConfigureBannerEvents();
         }

        // Ad event fired when the banner ad has loaded.
        public event EventHandler<EventArgs> OnAdLoaded;
        // Ad event fired when the banner ad has failed to load.
        public event EventHandler<YumiAdFailedToLoadEventArgs> OnAdFailedToLoad;
        // Ad event fired when the banner ad is click.
        public event EventHandler<EventArgs> OnAdClick;

        /// <summary>
        /// Loads an ad into the BannerView.
        /// </summary>
        public void LoadAd()
        {
            client.LoadAd(bannerOptions.isSmart);
        }
        /// <summary>
        /// Hides the BannerView from the screen.
        /// </summary>
        public void Hide()
        {
            client.HideBannerView();
        }

        /// <summary>
        /// Shows the BannerView on the screen.
        /// </summary>
        public void Show()
        {
            client.ShowBannerView();
        }

        /// <summary>
        /// Destroys the BannerView.
        /// </summary>
        public void Destroy()
        {
            client.DestroyBannerView();
        }

        private void ConfigureBannerEvents()
        {
            this.client.OnAdLoaded += (sender, args) =>
            {
                if (this.OnAdLoaded != null)
                {
                    this.OnAdLoaded(this, args);
                }
            };

            this.client.OnAdFailedToLoad += (sender, args) =>
            {
                if (this.OnAdFailedToLoad != null)
                {
                    this.OnAdFailedToLoad(this, args);
                }
            };

            this.client.OnAdClick += (sender, args) =>
            {
                if (this.OnAdClick != null)
                {
                    this.OnAdClick(this, args);
                }
            };


        }
        
        [Obsolete("LoadAd(bool isSmart) is deprecated, please use LoadAd() instead.", true)]
        public void LoadAd(bool isSmart)
        {
            client.LoadAd(isSmart);
        }
    }
}
