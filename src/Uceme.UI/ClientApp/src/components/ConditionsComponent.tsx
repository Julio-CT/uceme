import * as React from 'react';
import { Container } from 'reactstrap';
import './Home.scss';

const Conditions = (): JSX.Element => {
  return (
    <Container className="App header-distance">
      <div>
        De conformidad con lo establecido en el Art. 5 de la Ley Orgánica
        15/1999 de diciembre de Protección de Datos de Carácter Personal, por el
        que se regula el derecho de información en la recogida de datos le
        informamos de los siguientes extremos:
        <br />
        - Los datos de carácter personal que nos ha suministrado en esta y otras
        comunicaciones mantenidas con usted serán objeto de tratamiento en los
        ficheros responsabilidad de UCEME.
        <br />
        - La finalidad del tratamiento es la de gestionar de forma adecuada la
        prestación del servicio que nos ha requerido. Asimismo estos datos no
        serán cedidos a terceros, salvo las cesiones legalmente permitidas.
        <br />
        - Los datos solicitados a través de esta y otras comunicaciones son de
        suministro obligatorio para la prestación del servicio. Estos son
        adecuados, pertinentes y no excesivos.
        <br />
        - Su negativa a suministrar los datos solicitados implica la
        imposibilidad prestarle el servicio.
        <br />- Asimismo, le informamos de la posibilidad de ejercitar los
        correspondiente derechos de acceso, rectificación, cancelación y
        oposición de conformidad con lo establecido en la Ley 15/1999 ante UCEME
        como responsables del fichero. Los derechos mencionados los puede
        ejercitar a través de los siguientes medios: admin@uceme.es
      </div>
    </Container>
  );
};

export default Conditions;
