import React , {useState, useEffect} from "react";
import axios from "axios";
import './MyApp.css';
import Products from './Products.js';

function App(){;
    return <div id="SuperMarketApp">
        <Categories />
    </div>
}

function Categories(){
    const [categories,changeCategories] = useState(null);

    const [triggerEffect,changeTriggerEffect] = useState({value:true});
    
    useEffect(()=>{
        console.log("Effect used");
        updateCategories();
        triggerEffect.value=false;
    },[triggerEffect.value]);

    return <div>
        {categories}
        <br/>
        <AddCategoryForm triggerEffect={(x)=>{changeTriggerEffect(x)}}/>
    </div>

    function updateCategories(){
        getCategories().then((x)=>{
            let arr = [];
            try{
                x.data.forEach((y)=>{
                    arr.push(<CategoryDiv id={y.id} key={y.id} name={y.name}/>);
                });
            }catch(error){
                arr=[<h1>Could not connect to API</h1>];
            }
            changeCategories(arr);
        })
    }
    async function getCategories(){
        const response = await axios.get("http://localhost:5000/api/categories")
        .catch(function(error){
            console.log(error);
        });
        return response;
    }

    function CategoryDiv(props){
        let hidden = {
            display:"none"
        }
        let visible = {
            display:"block"
        }
    
        const [currentStyle,changeCurrentStyle] = useState(hidden);
        const [updated, changeUpdated] = useState(false);
        let div = <div>
            <h1 onClick={()=>changeVisibility()}>{props.name}</h1>
            <Products style={currentStyle} updated ={updated} changeUpdated={changeUpdated} categoryId={props.id}/>
        </div>
        return div;
    
        function changeVisibility(){
            changeUpdated(true);
            if (currentStyle.display=="block"){
                changeCurrentStyle(hidden);
            }else{
                changeCurrentStyle(visible);
            }
        }
    }
}


function AddCategoryForm(props){
    const [name, changeName] = useState("Category Name");

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Name:
                <input type="text" value={name} onChange={handleChange}/>
            </label>
            <input type="submit" value="Add Category" />
        </form>
    )

    function handleChange(event){
        changeName(event.target.value);
    }
    function handleSubmit(event){
        event.preventDefault();
        addCategory().then(()=>{
            props.triggerEffect({value:true});
        });
    }

    async function addCategory(){
        console.log(name);
        try {
            await axios.post(
                'http://localhost:5000/api/categories',
                { name: name }
            ).then((response) =>{
                console.log(response);
            });
        } catch(error){
            console.log(error);
        }
    }
}
export default App;