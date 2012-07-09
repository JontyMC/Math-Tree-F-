module MathTreeTests

open FsUnit.Xunit
open MathTree;
open Xunit;
open Xunit.Extensions;

[<Fact>]
let t() =
    let nodes = expressionToNodes "1-9/6*2+7"
    let tree = buildTree nodes Empty 0
    let output = evaluteTree tree

    output

[<Theory>]
[<InlineData("2+4+3", 9)>]
[<InlineData("2+4*3", 14)>]
[<InlineData("2/4*6", 3)>]
[<InlineData("2+4*3+5", 19)>]
[<InlineData("2+4*3-5", 9)>]
[<InlineData("3+4*5+7*3*5-9", 119)>]
[<InlineData("3+4*5+7*3*5-9/6*2+7", 132)>]
let ``Expression evaluates to expected value`` expression expected =
    evaluateExpression expression
    |> should equal (decimal expected)