# RedditReader

Sync posts for one Subreddit as fast as allowable by the RedditAPI rate limits.

**A couple quick notes on this code:**
* The sync operation is only looking at /new.json to poll for new posts.
* Existing posts will get updated when re-synced, but this is not actually scalable because once the script has been running for a while, the rate limiting would make it impossible to keep every post in memory up-to-date. For this reason, the "user w/ most posts" and "most upvoted post" stats aren't super useful because there is no way to keep all posts up-to-date given the rate limiting. Conversely, only adding new posts and getting stats based on only new posts is also not very useful because the stats would be based on the first time the post was synced.
* The main loop in the app is a while loop that will keep the Sync task and StatLogger task running in a loop. I want to call this out because this was coding explicitly to satisfy this requirement in the Coding Challenge description, "It’s very important that the various application processes do not block each other". In a real production use-case, if we had a script whose job was to sync posts, it would be run as a standalone script that did only that job, so blocking other application processes would not be a concern. Just wanted to point this out because there really wouldn't ever be a reason to code a script like this in a real-world scenario.
