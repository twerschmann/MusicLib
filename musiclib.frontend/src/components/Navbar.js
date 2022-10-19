import React from 'react'
import {useState} from 'react'
import {Button} from "@mui/material";


export default function Navbar() {
    const [user, setUser] = useState()

    const requestOptions = {
        method: 'POST',
        headers : {'Content-Type': 'application/json'},
        body: JSON.stringify({email:'tobi.werschi@gmail.com',password:'test123'})
    };


    fetch("https://localhost:7004/api/login",requestOptions)
        .then(response => response.json())
        .then(data => setUser(data.user))

  return (
    <Button onClick={() => alert("test")} variant="contained">{user.name}</Button>
  )
}
