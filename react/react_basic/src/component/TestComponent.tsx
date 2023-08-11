import React, {Component} from 'react';
import axios from "axios";
import {log} from "util";
import Loading from "./Loading";

interface MyComponentProps {
    // Define props here
    a: string,
    info: any,
}

interface MyComponentState {
    // Define state here
    b ?: string,
    users : [],
    loading : boolean
}

let product = {name: 'samsung', color: 'gold', price: 100};

class TestComponent extends Component<MyComponentProps, MyComponentState> {
    constructor(props: MyComponentProps) {
        // Initialize state and bind methods here if needed
        super(props);
        this.state = {b:'testState', users: [], loading : false}
    }

    componentDidMount() {
        // console.log(this.state);
        // Code to run after the component has been rendered
    }

    componentDidUpdate(prevProps: MyComponentProps, prevState: MyComponentState) {
        // Code to run after the component has been updated
    }

    componentWillUnmount() {
        // Code to run before the component is unmounted and destroyed
        this.getUsers();
    }

    getUsers(){
        this.setState({
            loading : true
        })

        axios('https://api.randomuser.me/?results=5').then(response => this.setState(
            {
                b: 'bbb',
                users : response.data.results,
                loading: false
            }
        ) );
    }

    changeInfo= () =>{
        this.setState(
            {b:'testState2'}
        );
    }

    render() {
        console.log(this.state.users)
        const {color, price} = product;
        const {name, email} = this.props.info;

        if(this.state.loading)
        {
            return (<Loading message="hey hey hey"/>);
        }
        else {
            return (
                <div>
                    {color} - {price}- {this.props.a} - {name} - {this.state.b}

                    <button onClick={this.changeInfo}>Change info</button>

                    <table className="table">
                        <thead>
                        <tr>
                            <th scope="col">Cell</th>
                            <th scope="col">Name</th>
                            <th scope="col">Gender</th>
                            <th scope="col">Email</th>
                        </tr>
                        </thead>
                        <tbody>

                        {this.state.users.map(user =>
                            <tr key={user['id']['value']}>
                                <th>{user['cell']}</th>
                                <td>{user['id']['value']}</td>
                                <td>{user['gender']}</td>
                                <td>{user['email']}</td>
                            </tr>
                        )}

                        {/*{*/}
                        {/*    !this.state.loading ? this.state.users.map(user =>*/}
                        {/*    <tr key={user['id']['value']}>*/}
                        {/*        <th>{user['cell']}</th>*/}
                        {/*        <td>{user['name']['first']}</td>*/}
                        {/*        <td>{user['gender']}</td>*/}
                        {/*        <td>{user['email']}</td>*/}
                        {/*    </tr>*/}
                        {/*): 'Loading'*/}
                        {/*}*/}
                        </tbody>
                    </table>
                </div>
            );
        }


    }
}

export default TestComponent;
