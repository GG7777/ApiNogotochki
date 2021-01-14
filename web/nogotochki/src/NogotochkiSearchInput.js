import './NogotochkiSearchInput.css';
import { useState } from 'react';
import NogotochkiButton from './NogotochkiButton';
import NogotochkiTextInput from './NogotochkiTextInput';

function NogotochkiSearchInput(props) {
    const [text, setText] = useState('');

    function onTextChange(e) {
        let text = e.target.value;

        setText(text);
    }

    return (
        <div className={`nogotochki-search-input-container ${props.className}`}>
            <NogotochkiTextInput
                className="input-element item-element"
                placeholder="Введите запрос"
                onChange={onTextChange}
            >
                {text}
            </NogotochkiTextInput>
            <NogotochkiButton
                className="search-element item-element"
                onClick={e => props.onSearch(text)}
            >
                Найти
            </NogotochkiButton>
        </div>
    );
}

export default NogotochkiSearchInput;