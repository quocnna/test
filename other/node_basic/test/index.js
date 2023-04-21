const http = require("http");
const fs = require("fs");
const path = require("path");
const todoController = require("./controllers/todoController");

const router = {
  get: {
    "/todo/list": todoController.getListTodo,
    "/todo/new": todoController.getNewTodoForm,
  },
  post: {
    "/todo/new": todoController.createNewTodo,
  },
};

const server = http.createServer(async (req, res) => {
  const method = req.method;
  const url = req.url;

  // Home page
  if (url === "/") {
    // const homePageData = fs.readFileSync(path.resolve(__dirname, './views/home.html'));
    // console.log('LOG HERE');
    // res.write(homePageData.toString());
    // return res.end();

    try {
      const data = await (() => {
        return new Promise((resolve, reject) => {
          fs.readFile(
            path.resolve(__dirname, "./views/home.html"),
            (err, data) => {
              if (err) {
                reject(err);
              } else {
                resolve(data);
              }
            }
          );
        });
      })();

      res.write(data);
      return res.end();
    } catch (err) {
      // Has Error
      res.write("<h1>Internal Server Error</h1>");
      return res.end();
    }
  }

  if (url === '/favicon.ico') {
    return res.end();
  }

  if (url.startsWith('/css')) {
    const data = fs.readFileSync(path.resolve(__dirname, `.${url}`));
    res.write(data);
    return res.end();
  }

  try {
    return router[method.toLowerCase()][url](req, res);
  } catch (err) {
    console.log(url);
    console.log(err);
    res.write(`
        <h1>404 NOT FOUND</h1>
    `);
    return res.end();
  }
});

const PORT = 3000;

server.listen(PORT, () => {
  console.log("Server is running on port: " + PORT);
});
