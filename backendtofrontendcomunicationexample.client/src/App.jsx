import { useEffect, useState } from 'react';
import './App.css';
import * as signalR from "@microsoft/signalr";

function App() {
    const [entries, setEntries] = useState();
    const [connected, setConnected] = useState(false);
    const [connectionSaved, setConnectionSaved] = useState(null);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7031/eventEntryHub", {
                transport: signalR.HttpTransportType.WebSockets, 
                withCredentials: true // Allow credentials
            })
            .configureLogging(signalR.LogLevel.Debug)
            //.withAutomaticReconnect()
            .withAutomaticReconnect()
            .build();

        //newConnection.onclose((error) => {
        //    //console.assert(newConnection.state === signalR.HubConnectionState.Disconnected);
        //    //  connection.start()
        //    console.log(`Connection closed due to error: "${error}".`);
        //    setErrorMessage(`Connection closed due to error: "${error}". Try refreshing this page to restart the connection.`);
        //});

        connection.start().then(() => {
            connection.invoke("GetConnectionId").then((connectionId) => { console.log("connectionId", connectionId); });
            console.log("SignalR Connected");
            setConnected(true);
        }).catch(err => console.error("SignalR Connection Error: ", err));

        connection.on("ConnectionEst", (status) => {
            console.log("ConnectionEst: ", status);

            // return () => connection.stop();
        }, []);

        connection.on("ReceiveEntryEvent", (type, updatedObject) => {
            console.log("Received entry event: ", type, updatedObject);
            setEntries(prevData => {
                const index = prevData?.findIndex(item => item.id === updatedObject.id);
                if (index !== -1) {
                    return prevData?.map(item => item.id === updatedObject.id ? updatedObject : item);
                }
                return [...prevData, updatedObject];
            });
        });

        setConnectionSaved(connection);

        return () => connection.stop();
        }, []);
   

    useEffect(() => {
        populateEntryData();
    }, []);

    const contents = entries === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name. (C)</th>
                    <th>Start Date (F)</th>
                </tr>
            </thead>
            <tbody>
                {entries.map(entry =>
                    <tr>
                        <td>{entry.id}</td>
                        <td>{entry.name}</td>
                        <td>{entry.starTime}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Entry Info</h1>
            <p>This component demonstrates fetching data from the server and getting updates.</p>
            {contents}
        </div>
    );
    
    async function populateEntryData() {
        const response = await fetch('entries');
        if (response.ok) {
            const data = await response.json();
            console.log('aaaaaaaaaaaaaaaaaaaaaaa');
            setEntries(data);
        }
    }
}

export default App;