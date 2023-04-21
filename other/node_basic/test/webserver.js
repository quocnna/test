const http = require("http");

const server = http.createServer((req, res) => {
    // do something
    res.write("<h1>Hello world</h1>");
    res.write(`
    <ul>
        <li>Node.js</li>
        <li>ReactJS</li>
    </ul>
  `);
    return res.end();
});

const PORT = 3000;

server.listen(PORT, () => {
    console.log("Server is running on port " + PORT);
});
