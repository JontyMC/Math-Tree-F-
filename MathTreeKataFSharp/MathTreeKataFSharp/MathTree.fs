module MathTree

open System;

type Operator = | Add | Subtract | Multiply | Divide

type Node =
    | Empty
    | Constant of decimal
    | Node of Node * Operator * Node

let rec buildTree nodeList tree precendence =
    let opFromNode node =
        match node with
        | Node (_, op, _) -> op
        | _ -> failwith "Invalid expression"

    let operatorPrecedence op =
        match op with
        | Add -> 0
        | Subtract -> 0
        | Multiply -> 1
        | Divide -> 1

    match nodeList with
        | a::b::tail ->
            let nextOp = opFromNode b
            let operatorPrecedence = operatorPrecedence nextOp

            match tree with
                | Empty -> buildTree tail (Node (a, nextOp, Empty)) operatorPrecedence
                | Node (l, op, Empty) ->
                    if operatorPrecedence > precendence then
                        buildTree tail (Node (l, op, (buildTree tail (Node (a, nextOp, Empty)) operatorPrecedence))) precendence
                    else if operatorPrecedence = precendence then
                        buildTree tail (Node (Node (l, op, a), nextOp, Empty)) precendence
                    else
                        Node (l, op, a)
                | Node (l, op, r) ->
                    if operatorPrecedence > precendence then
                        buildTree tail tree precendence
                    else
                        buildTree tail (Node (tree, nextOp, (buildTree tail Empty precendence))) precendence
                | _ -> failwith "Invalid expression"
                        
        | [a] ->
            match tree with
            | Empty -> a
            | Node (l, op, Empty) -> Node (l, op, a)
            | _ -> tree
        | _ -> failwith "Invalid expression"

let expressionToNodes expression =
    let characterToNode character =
        match character with
            | character when Char.IsDigit(character) -> Constant (decimal (Char.GetNumericValue(character)))
            | _ ->
                match character with
                    | '+' -> Node (Empty, Add, Empty)
                    | '-' -> Node (Empty, Subtract, Empty)
                    | '*' -> Node (Empty, Multiply, Empty)
                    | '/' -> Node (Empty, Divide, Empty)
                    | _ -> failwith "Invalid operator"

    List.map characterToNode (expression |> Seq.toList) 

let rec evaluteTree tree =
    let operatorFunction operator =
        match operator with
            | Add -> fun x y -> x + y
            | Subtract -> fun x y -> x - y
            | Multiply -> fun x y -> x * y
            | Divide -> fun x y -> x / y

    match tree with
        | Empty -> 0m
        | Constant x -> x
        | Node (node1, operator, node2) ->
            let operator1 = operator
            let operatorFunction = operatorFunction operator
            let node1 = evaluteTree node1
            let node2 = evaluteTree node2
            let result = operatorFunction node1 node2

            result

let evaluateExpression expression =
    let nodes = expressionToNodes expression
    let tree = buildTree nodes Empty 0

    evaluteTree tree


