const fs = require("fs");
const path = require("path");
/**
 * Note: GV đang sử dụng node version cũ 16.16.0 nên không hỗ trợ module `qs`.
 *
 * Bạn nào đang sử dụng node version mới hơn có thể kiểm tra thử nếu version đó không hỗ trợ thì cài thêm `querystring`
 * */
const qs = require("querystring");

const LIST_TODO_DATA = [
    {
        id: 1,
        name: "Learn Node.js",
    },
];

const readFileInSystem = (pathname) => {
    return new Promise((resolve, reject) => {
        fs.readFile(
            path.resolve(__dirname, pathname),
            { encoding: "utf-8" },
            (err, data) => {
                if (err) {
                    reject(err);
                } else {
                    resolve(data);
                }
            }
        );
    });
};

const getListTodo = async (req, res) => {
    const data = await readFileInSystem("../views/todo/list.html");

    let listTodoStr = "";

    for (let todo of LIST_TODO_DATA) {
        listTodoStr += `
        <div class="todo">
         #${todo.id} - ${todo.name}
        </div>
    `;
    }

    res.write(data.replace("{listTodo}", listTodoStr));
    return res.end();
};

const getNewTodoForm = async (req, res) => {
    const data = await readFileInSystem("../views/todo/createForm.html");
    res.write(data);
    return res.end();
};

const createNewTodo = (req, res) => {
    let dataChunk = "";
    req.on("data", (chunk) => {
        dataChunk += chunk;
    });

    req.on("end", () => {
        const newTodo = qs.parse(dataChunk);
        LIST_TODO_DATA.push({
            ...newTodo,
            id: LIST_TODO_DATA.length + 1,
        });
        res.writeHead(302, {
            Location: "/todo/list",
        });
        return res.end();
    });
};

module.exports = {
    getListTodo,
    getNewTodoForm,
    createNewTodo,
};
