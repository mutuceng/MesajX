import { useState } from 'react'
import './App.css'
import Chat from "./signalR/Chat";
import Sidebar from './components/sidebar/sidebar';

function App() {
  

  return (
    <>
      <div className='sidebar'>
        <Sidebar/>
      </div>
      <div>
          <h1>SignalR ile Gerçek Zamanlı Chat</h1>
          <Chat />
      </div>
      
    </>
  )
}

export default App
