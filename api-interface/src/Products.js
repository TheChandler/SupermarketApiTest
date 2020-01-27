import React , {useState, useEffect} from "react";
import axios from "axios";
import './MyApp.css';

function Products(props){
    const [products, changeProducts] = useState(null);
    
    useEffect(()=> {
        console.log(props);
        getProducts().then((x)=>{
            let arr = [];
            x.data.forEach((y)=>{
                if (y.category.id==props.categoryId)
                    arr.push(<ProductDiv key={y.id} id={y.id} quantityInPackage={y.quantityInPackage} unitOfMeasurement={y.unitOfMeasurement} name={y.name} style={props.style} />);
            });
            changeProducts(arr);
            props.changeUpdated(false);
        });
    },[props.updated]);

    return <div style={props.style}>
        {products}
        <AddProductForm changeUpdated={props.changeUpdated} categoryId={props.categoryId}/>
    </div>;

    function ProductDiv(props){
        let div = <div style={props.style}>
            <h3>{props.quantityInPackage} {props.name}</h3>
        </div>
        return div;
    }

    async function getProducts(){
        const response = await axios.get("http://localhost:5000/api/products")
        .catch(function(error){
            console.log(error);
        });
        return response
    }
}

function AddProductForm(props){
    const [name,changeName] = useState("Product Name");
    const [numInPackage,changeNumInPackage] = useState(1);

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Name:
                <input type="text" value={name} onChange={nameChange}/>
            </label>
            <br/>
            <label>
                Amount in Package:
                <input type="number" max={30000} min={-30000} value={numInPackage} onChange={numChange}/>
            </label>
            <br/>
            <input type="submit" value="Add Product" />
        </form>
    )

    function handleSubmit(event){
        event.preventDefault();
        AddProduct().then(()=>{
            props.changeUpdated(true);
        });
    }
    
    function nameChange(event){
        changeName(event.target.value);
    }
    function numChange(event){
        console.log(numInPackage);
        changeNumInPackage(Number(event.target.value));
        
    }

    async function AddProduct(){
        console.log(props.categoryId);
        try {
            await axios.post(
                'http://localhost:5000/api/products',
                { 
                    name : name,
                    quantityInPackage: numInPackage,
                    unitOfMeasurement: 1,
                    categoryid: props.categoryId
                }
            ).then((response)=>{
                console.log(response);
            });
        } catch(error){
            console.log(error);
        }
    }
}

export default Products;