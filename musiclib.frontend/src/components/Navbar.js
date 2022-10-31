import React, { useRef } from 'react'
import {useState} from 'react'
import {Button, TextField} from "@mui/material";


export default function Navbar() {
    let [counter, setCounter] = useState(0)
    let [user,setUser] = useState([]);


    function login(e){
      e.preventDefault();
      
      const eMail = e.target.email.value;
      const passWord = e.target.password.value;

      const requestOptions = {
        method: 'POST',
        headers : {
          'Content-Type': 'application/json',
          // "Access-Control-Allow-Origin": "*",
          // "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
        },
        body: JSON.stringify({email: eMail, password: passWord})
      };

      fetch("https://localhost:7004/api/login",requestOptions)
        .then(response => response.json())
        .then(data => {

          if(data.user != null){
            setUser(data.accessToken)
          }
          else{
            setUser('Falsche Credentials');
          }

        })
    }

    function hochzählen(){
      setCounter(count => count + 1);

      login();
    }
    
  return (
    <div className='container'>
    
    <p>{user}</p>
      <form onSubmit={login}>
      <TextField name="email" type="email" label="Email" variant="outlined" />
      <TextField name="password" type="password" label="Passwort" variant="outlined" />
      <Button variant="contained" type="submit">Login</Button>
      </form>
    </div>
  )
}


