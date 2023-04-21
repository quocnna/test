// setTimeout(() => {
//     console.log("Say hello");
// });

// console.log("Good bye");

// axios
const axios = require("axios").default;

// function fetchPosts() {
//     // // web apis => fetch
//     // fetch('https://my-json-server.typicode.com/typicode/demo/posts')
//     //     .then((res) => res.json()).then((data) => console.log(data));

//     axios.get('https://my-json-server.typicode.com/typicode/demo/posts').then((res) => {
//         const data = res.data;
//         console.log(data);
//     });
//     console.log("LOG HERE 1");
// }

function fetchPosts() {
    // // web apis => fetch
    // fetch('https://my-json-server.typicode.com/typicode/demo/posts')
    //     .then((res) => res.json()).then((data) => console.log(data));
    // axios.get('https://my-json-server.typicode.com/typicode/demo/posts').then((res) => {
    //     const data = res.data;
    //     console.log(data);
    // });

    return new Promise(async (resolve, reject) => {
        try {
            const res = await axios.get(
                "https://jsonplaceholder.typicode.com/users"
            );
            const data = res.data;
            console.log(data);
            console.log("LOG HERE 1");
            resolve();
        } catch (error) {
            reject(error.message);
        }
    });
}

(async () => {
    try {
        await fetchPosts();
    } catch (error) {
        console.log(error);
    }
})();

// fetchPosts().catch((error) => console.log(error));

// try {

// } catch (error) {
//   console.log("ERROR: " + error.message);
// }
