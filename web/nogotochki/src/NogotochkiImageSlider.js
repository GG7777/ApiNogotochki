import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import './NogotochkiImageSlider.css';
import Slider from 'react-slick';

function NogotochkiImageSlider(props) {
    let settings = {
        dots: true,
        infinite: true,
        slidesPerRow: 2,
    }

    return (
        <div className="nogotochki-image-slider">
            <Slider {...settings}>
                {props.images.map((x, i) => (
                    <div className="item-container" key={i}>
                        <div className="image item">
                            <img height="450px" src={x.path} alt={x.title} />
                        </div>
                        <div className="description-container item">
                            <h3 className="title">
                                {x.title}
                            </h3>
                            <p className="description">
                                {x.description}
                            </p>
                        </div>
                    </div>
                ))}
            </Slider>
        </div>
    );
}

export default NogotochkiImageSlider;