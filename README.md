# StructuralEqualityAssesor
A .Net core library to asseses if structural equality has been properly implemented in a namespace. 

## Thanks
[EDF-RE](https://github.com/edf-re) : For giving me time to create and maintain this.

## Example

```csharp
using Xunit;

namespace MyCompany.MyNamespace.Tests
{
    public class ModelsShould
    {
        // This test fails if any type in the 'MyCompany.MyNamespace' does not 
        // correctly impliment structural equality.
        [Fact]
        public void ImplementStructuralEquality()
        {
            // Ignore types that cannot be compared or do not whish to have structural
            // equality.
            var ignoreThese = new[] {typeof(MyCompany.MyNamespace.NoEqualityType)};
            var badTypes = 
                FindSiblingsThatAreMissingStructural
                    .Equality<MyCompany.MyNamespace.MyClass>(ignoreThese);

            // This will output a list of types that do not have structural 
            // equality correctly implimented and where not ignored.
            Assert.Empty(badTypes);
        }
    }
}
```



## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="http://www.chrisstead.net/"><img src="https://avatars3.githubusercontent.com/u/4184510?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Chris Stead</b></sub></a><br /><a href="#ideas-cmstead" title="Ideas, Planning, & Feedback">🤔</a></td>
    <td align="center"><a href="https://github.com/edf-re"><img src="https://avatars0.githubusercontent.com/u/13739273?v=4?s=100" width="100px;" alt=""/><br /><sub><b>EDF Renewables</b></sub></a><br /><a href="#financial-edf-re" title="Financial">💵</a></td>
  </tr>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!