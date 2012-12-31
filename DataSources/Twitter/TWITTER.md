# Twitter

## Overview

The Twitter data source uses the Twitter API. Access to Twitter is facilitated through a dependency
on the [Twitterizer library](http://www.twitterizer.net/).

Usage of the Twitter API requires developer setup and configuration for a Twitter account. The 
[Twitter Developer portal](https://dev.twitter.com/) contains information for getting 
established to use the Twitter API.

## Configuration
```
    <Source name="Twitter" type="GreenlakeChristmas.RDSFeed.DataSources.Twitter.TwitterDataSource">
      <RefreshInterval>30</RefreshInterval>
      <Priority>General</Priority>
      <Constructor>
        <Parameter name="accesstoken" value="{your-access-token}" />
        <Parameter name="accesstokensecret" value="{your-access-token-secret}" />
        <Parameter name="consumerkey" value="{your-consumer-key}" />
        <Parameter name="consumersecret" value="{your-consumer-secret}" />
      </Constructor>
      <Options>
        <Parameter name="hashtag" value="christmas" />
      </Options>
      <Templates rotation="Random">
        <Case when="Mention" method="GetMention" template="{2}({0})" />
        <Case when="Retweet" method="GetRetweet" template="{2}({0})" />
        <Case when="Tweet" method="GetTweet" template="{2}({0})" />
        <Case when="Hashtag" method="GetHashTweet" template="{2}({0})" />
      </Templates>
    </Source>
```

## Constructor parameters

* accesstoken - the access token, provided by Twitter.
* accesstokensecret - the access token secret, provided by Twitter.
* consumerkey - the consumer key, provided by Twitter.
* consumersecret - the consumer secret, provided by Twitter.

## Options parameters

* hashtag - if supplied, enables the GetHashTweet method to return data.

## Template methods

[GetMention](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Twitter/TwitterDataSource.cs#L28)

#### Returns

String array, index 0: the tweet creation date.
              index 1: the tweet author.
              index 2: the tweet text.

---

[GetRetweet](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Twitter/TwitterDataSource.cs#L45)

#### Returns

String array, index 0: the tweet creation date.
              index 1: the tweet author.
              index 2: the tweet text.

---

[GetTweet](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Twitter/TwitterDataSource.cs#L61)

#### Returns

String array, index 0: the tweet creation date.
              index 1: the tweet author.
              index 2: the tweet text.

---

[GetHashTweet](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Twitter/TwitterDataSource.cs#L77)

GetHashTweet is dependent on the presence of &lt;Options&gt; parameter named "hashtag".

#### Returns

String array, index 0: the tweet creation date.
              index 1: the tweet author.
              index 2: the tweet text.
