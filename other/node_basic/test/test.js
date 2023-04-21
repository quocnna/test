// console.log("hello world")
//
// const fetchRandomUser = () => {
//     fetch("https://jsonplaceholder.typicode.com/users").then(response => {
//         const data = response.json()
//         console.log({ data })
//         console.log("3");
//     });
//
//     console.log("1")
// }
//
// fetchRandomUser()
//
// console.log("2")

const fs = require("fs");
const path = require("path");

async function main() {
    const data =  await (() => {
        return    fetch("https://jsonplaceholder.typicode.com/users")
    })();

    console.log("data");
}


// main();
// console.log("1")

(async () => {
     await main();
    console.log("1")
})();

// (async () => {
//     try {
//         await main();
//         console.log("1")
//     } catch (error) {
//         console.log(error);
//     }
// })();

// async function test(){
//     await main();
//     console.log("1")
// }

// test();
