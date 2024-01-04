## Scannect

This was an interesting project that required several steps in order to produce some rudimentary version of a search engine.

The front end was a simple ASP.NET web application with a search bar...

![Untitled](https://github.com/tomfrancix/scannex/assets/58977284/34ec009c-8065-49b5-9fdf-9bd367fd9182)

To set up the database:

- Clone it
- Run the website, and register a new user.
- Open the Nuget Package manager and run `Update-Database -Context ScannectContext`

Then run the 'scraper' console application where you will have the option to scrape any URL. 

![Untitled1](https://github.com/tomfrancix/scannex/assets/58977284/d0ecf4dc-5964-45dc-9a99-bfbf9334b402)

The URL metadata will be saved to an S3 bucket, and a list of all the URLs discovered on the page are saved to another bucket.

Each URL discovered is then visited causing a continuous cycle of scraping... (I know... it's a bit insane...)

Then, run the 'vortex' console application to process all the payloads in S3.

They can both run simultaneously.

Then, run the website and search for something...

![Untitled3](https://github.com/tomfrancix/scannex/assets/58977284/019e65cc-a57e-4d37-9ebb-913585c94bf2)

You can also see images that are scraped (this is a bad example):

![Untitled4](https://github.com/tomfrancix/scannex/assets/58977284/1015525b-f3f7-4490-bf3d-e7a31fd43a28)
