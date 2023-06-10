import {Client} from '@stomp/stompjs';
import { WebSocket } from 'ws';
Object.assign(global, { WebSocket });

const client = new Client({
    brokerURL: 'ws://localhost:61614/ws',
    connectHeaders: {
        login: 'smx',
        passcode: 'smx',
    },
    debug: function (str) {
        console.log(str);
    },
    reconnectDelay: 5000,
    heartbeatIncoming: 4000,
    heartbeatOutgoing: 4000,
});

client.onConnect = function (frame) {
    console.log("Connected")

    const quote = {
        "safeIdEmpfaenger": "DE.Justiztest.16ef6e0e-4a87-478c-8dc3-6c640b8e6949.a12f",
        "azEmpfaenger": "",
        "mandant": "f08d2d54-1ab8-5e47-8d53-ca514e25bb4b",
        "mainObjId": 145,
        "parentObjId": 142,
        "objIds": [
            312,
            314,
            145,
            146,
            311,
            313
        ],
        "transferWritePermissions": [
            146,
            311,
            312
        ],
        "deliveryReason": "",
        "docTransfer": false
    }
    // client.publish({
    //     destination: '/queue/JUSTIZ_TRANSFER',
    //     body: JSON.stringify(quote),
    //     headers: { 'content-type': 'application/json' },
    // });

    client.publish({
        destination: '/queue/JUSTIZ_TRANSFER',
        body: JSON.stringify(quote),
        headers: { priority: '9' },
    });


    // const quote = { symbol: 'AAPL', value: 195.46 };
    // client.publish({
    //     destination: '/queue/qtest',
    //     body: JSON.stringify(quote),
    //     headers: { 'content-type': 'application/json' },
    // });
};



client.onStompError = function (frame) {
    console.log('Broker reported error: ' + frame.headers['message']);
    console.log('Additional details: ' + frame.body);
};

client.activate();
