[AppVeyorProjectUrl]: https://ci.appveyor.com/project/GregTrevellick/solutionopenpopup
[AppVeyorProjectBuildStatusBadgeSvg]: https://ci.appveyor.com/api/projects/status/ap87wkdaam6jkgui?svg=true
[GitHubRepoURL]: https://github.com/GregTrevellick/SolutionOpenPopUp
[GitHubRepoIssuesURL]: https://github.com/GregTrevellick/SolutionOpenPopUp/issues
[GitHubRepoPullRequestsURL]: https://github.com/GregTrevellick/SolutionOpenPopUp/pulls
[VersionNumberBadgeURL]: https://vsmarketplacebadge.apphb.com/version/GregTrevellick.SolutionOpenPopUp.svg
[VisualStudioURL]: https://www.visualstudio.com/
[VSMarketplaceUrl]: https://marketplace.visualstudio.com/items?itemName=GregTrevellick.SolutionOpenPopUp
[VSMarketplaceReviewsUrl]: https://marketplace.visualstudio.com/items?itemName=GregTrevellick.SolutionOpenPopUp#review-details


# SolutionOpenPopUp

[![Licence](https://img.shields.io/github/license/gittools/gitlink.svg)](/LICENSE.txt)
[![Build status][AppVeyorProjectBuildStatusBadgeSvg]][AppVeyorProjectUrl]
[![][VersionNumberBadgeURL]][VSMarketplaceUrl]
<!--![](https://vsmarketplacebadge.apphb.com/installs/GregTrevellick.SolutionOpenPopUp.svg)-->
<!--![](https://vsmarketplacebadge.apphb.com/rating/GregTrevellick.SolutionOpenPopUp.svg)-->

Download this extension from the [VS Marketplace][VSMarketplaceUrl].

---------------------------------------

<!--COPY START FOR VS GALLERY-->

This [Visual Studio][VisualStudioUrl] extension will open a pop-up dialog containing the contents of a specific text file in a solution, when the solution is opened.

 - *If you like this ***free*** tool please take a few seconds out to give a star rating below*.

## Use-Cases

 - Your source-control system freakishly doesn't retained a file's modifications, and so every time you open a new branch of your solution you need to manually and repeatedly make the same changes to address this. *This was the real-life problem I experienced that inspired me to create this extension.*
 
 - Many more. *If you have a use-case you would like listed here, just let me know via a review on the [VS Gallery][VSMarketplaceReviewsUrl].*
 
## Features

- Modifications can be limited to files under source control only. This is the default setting, but can be overridden to allow non-source controlled files to be modified.

- If a solution is opened whose name matches a rule, user is shown a summary of any modifications made. This summary can be surpressed if no modifications were made.

<!--COPY END FOR VS GALLERY-->

## Contribute

Contributions to this project are welcome by raising an [Issue][GitHubRepoIssuesURL] or submitting a [Pull Request][GitHubRepoPullRequestsURL].

## License

[MIT](/LICENSE.txt)

## Credits

The following authors / articles deserve special mention for their help whilst creating this extension:

[Mads Kristensen](https://channel9.msdn.com/Events/Build/2016/B886)

[Joshua Thompson](http://schmalls.com/2015/01/19/adventures-in-visual-studio-extension-development-part-2)

[Slaks.Blog](http://blog.slaks.net/2013-11-10/extending-visual-studio-part-2-core-concepts/)

[Daniel Schroeder](http://blog.danskingdom.com/category/visual-studio-extensions/)

[Sho Sato](https://vsmarketplacebadge.apphb.com/)

[![](chart.png)][VSMarketplaceUrl]
