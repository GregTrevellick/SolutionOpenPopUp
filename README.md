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

Display the content of one or more text files in a pop-up window when opening a solution. A handy way to share solution-specific gotchas, non-critical solution-specific messages within a team, your own personal comments about a solution, and more. 

 - *If you like this ***free*** extension please give a star rating below, it only takes a few seconds*.

## Use-Cases

 - We're planning to upgrade this solution to VS2017 in the next quarter, so if you haven't already please download and install VS2017

 - Gotcha: this solution has a build step that is dependant on a specific hard coded file path existing on your PC, without which it won't compile

 - This solution contains the public facing API that allows authorised customers to inject new products into the database 
 
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

[![](./chart.png)][VSMarketplaceUrl]
