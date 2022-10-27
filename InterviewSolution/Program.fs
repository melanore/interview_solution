open System

// more efficient space complexity 
let getMaxSubstringV1 (word1 : string) (word2 : string) : string =
    let mutable slice = ReadOnlySpan.Empty
    for i in 0..word1.Length - 1  do
        for j in 0..word2.Length - 1 do
            let mutable k = 0
            while word1.Length > i + k &&
                  word2.Length > j + k &&
                  word1[i + k] = word2[j + k] do
                k <- k + 1
            if slice.Length < k then
                slice <- word1.AsSpan().Slice(i, k)
    slice.ToString()

(* more efficient time complexity
|   | A | P | A | D | N |
| A | 1 | 0 | 1 | 0 | 0 |
| D | 0 | 0 | 0 | 2 | 0 |
| N | 0 | 0 | 0 | 0 | 3 |
| F | 0 | 0 | 0 | 0 | 0 | 
*)
let getMaxSubstringV2 (word1 : string) (word2 : string) : string =
    let matrix = Array2D.zeroCreate word1.Length word2.Length
    let mutable slice = ReadOnlySpan.Empty
    for i in 0..word1.Length - 1 do
        for j in 0..word2.Length - 1 do
            if word1[i] = word2[j] then
                let prev = if i = 0 || j = 0 then 0 else matrix[i-1, j-1]
                let value = 1 + prev
                matrix[i,j] <- value
                if slice.Length < value then
                    slice <- word1.AsSpan().Slice(i - value + 1, value)
    slice.ToString()

let testDataset = [
    "potato", "at" 
    "at", "potato"
    "pad", "adf"
    "0match0match0mat", "1match1match0ma"
    "0ch0ma0t", "1ch1ma0mat"
]

let printResults (version: string) (findMaxSubstring : string -> string -> string) word1 word2 =
    let substring = findMaxSubstring word1 word2
    let steps = word1.Length  + word2.Length - 2 * substring.Length        
    printfn $"| %s{version} | %s{word1} | %s{word2} | %A{substring} | %i{steps} |"
 
printfn "| version | word1 | word2 | substring | steps |"
printfn "|---|---|---|---|---|"   
for word1, word2 in testDataset do
    printResults "v1" getMaxSubstringV1 word1 word2
    printResults "v2" getMaxSubstringV2 word1 word2