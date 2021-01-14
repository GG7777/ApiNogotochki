import './App.css';
import Header from './Header';
import Body from './Body';
import Footer from './Footer';
import { BrowserRouter } from 'react-router-dom';

function App() {
    return (
        <div className="app-container">
            <BrowserRouter>
                <Header />
                <Body />
                <Footer />
            </BrowserRouter>
        </div>
    );
}

export default App;