import * as React from 'react';
import parse from 'html-react-parser';
import './Blogs.scss';
import tinyDate from '../../resources/images/tinydate.png';
import photoIcon from '../../resources/images/photoicon.png';

interface blog {
    title: string;
    src: string;
    altText: JSX.Element | JSX.Element[];
    caption: string;
    link: string;
    date: string;
}

type blogState = {
    items: blog[];
    isFetching: boolean;
};

const Blogs = (props: any) => {
    const [data, setData] = React.useState<blogState>({ items: [] as Array<blog>, isFetching: false });
    const [loading, setLoading] = React.useState(true);

    const fetchData = () => {
        if (loading) {
            fetch('blog/getblogsubset?amount=3')
                .then((response) => response.json())
                .then((data) => {
                    const retrievedBlogs: blog[] = [];
                    data.forEach((obj: any) => {
                        retrievedBlogs.push({
                            title: obj.titulo,
                            src: require('../../resources/images/icon-spe1.png').default,
                            altText: parse(obj.texto),
                            caption: obj.titulo,
                            link: 'Cirugía de Glándulas Suprarrenales',
                            date: new Intl.DateTimeFormat("en-GB", {
                                year: "numeric",
                                month: "long",
                                day: "2-digit"
                              }).format(obj.date),
                        });
                    });

                    setData({ items: retrievedBlogs, isFetching: false });
                    setLoading(false);
                })
                .catch((error) => {
                    console.log(error);
                    setLoading(false);
                });
        };
    };

    React.useEffect(() => {
        fetchData();
    });

    const posts = data.items.map((item) => {
        return (
            <div className='blogs col-12 col-md-4'>
                <div className='box-noticias' key={item.title}>
                    <a href={item.link}>
                        <img
                            src={item.src} alt={item.title}
                            className='img-post wp-post-image'
                            width='370' height='230' />
                    </a>
                    <a href={item.link}>
                        <img
                            className='icono-img' src={photoIcon} alt='icon' />
                    </a>
                    <h5 className='uppercase'>
                        <a href={item.link}>
                            {item.caption}
                        </a>
                    </h5>
                    <div className='line-small'></div>
                    <p></p>
                    <p className="post-ellipsis">{item.altText}</p>
                    <p></p>
                </div>
                <div className='box-admin'>
                    <div className='date uppercase'>
                        <img src={tinyDate} alt='date' />{item.date}
                    </div>
                </div>
            </div>
        )
    });

    return (
        <section id='section-blog' className='clearfix'>
            {loading ? (
                <div>...Data Loading.....</div>
            ) : (
                    <div className='blog container clearfix extra-margin'>
                        <h3 className='uppercase'>BLOG</h3>
                        <h4 className='padding-y-medium spacing uppercase'>
                            Actualidad, agenda y noticias.
            </h4>
                        <div className='line'></div>
                        <div className='row justify-content-md-center'>
                            {posts}
                        </div>
                    </div>
                )}
        </section>
    );
}

export default Blogs;
