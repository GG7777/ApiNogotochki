import './Title.css';

function Title(props) {
    return (
        <div class="main-title">{props.children}</div>
    );
}

export default Title;