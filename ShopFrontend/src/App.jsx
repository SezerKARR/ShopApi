import React from 'react'
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import './App.css'
import axios from "axios";
import Header from "./Components/Header/Header.jsx";
import Home from "./Pages/Home.jsx";

function App() {
    return (

        <Router>
            <div className={"page"}>
                <header>
                    <Header/>
                </header>
                <main className={"content"}>

                    <Routes>
                        <Route path="/" element={<Home/>}/>
                        {/*<Route path="/Category/:categorySlug" element={<Saplings/>}/>*/}
                        {/*<Route path="/Sapling/:saplingSlug" element={<Sapling/>}/>*/}
                    </Routes>

                </main>
            </div>
        </Router>

    );
    // return (
    //
    //     <div className="App">
    //       
    //         <main >
    //             <Router>
    //                 <Routes>
    //                     <Route path="/" element={<Home/>}/>
    //                     {/*<Route path="/Category/:categorySlug" element={<Saplings/>}/>*/}
    //                     {/*<Route path="/Sapling/:saplingSlug" element={<Sapling/>}/>*/}
    //                 </Routes>
    //             </Router>
    //           
    //         </main>
    //     </div>
    //
    //
    // )
}

export default App
